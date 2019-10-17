using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using edu_croaker.DataAccess;
using edu_croaker.Data;
using edu_croaker.Dtos;

namespace edu_croaker.Services
{
    public class CroakService
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;

        public const int MAX_POPULAR_HASHTAGS = 5;

        public CroakService(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksAsync()
        {
            var croaks = await _repo.FindCroaks();

            var croakDtos = croaks
                .Select(x => _mapper.Map<CroakDto>(x))
                .Reverse();

            return await GetWithUserNamesAsync(croakDtos);
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksWithHashtagAsync(string caption)
        {
            var hashtag = await _repo.FindHashtag(caption);
            var ids = await _repo.FindCroakIdsWithHashtag(hashtag.Id);
            var croaks = await _repo.FindCroaks(ids);

            var croakDtos = croaks
                .Select(x => _mapper.Map<CroakDto>(x))
                .Reverse();

            return await GetWithUserNamesAsync(croakDtos);
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksByAuthorAsync(string authorName)
        {
            var croaks = await _repo.FindCroaksByAuthor(authorName);

            var croakDtos = croaks
                .Select(x => _mapper.Map<CroakDto>(x))
                .Reverse();

            return await GetWithUserNamesAsync(croakDtos);
        }

        protected Task<CroakDto[]> GetWithUserNamesAsync(IEnumerable<CroakDto> croakDtos)
        {
            return Task.WhenAll(croakDtos
                .Select(async x =>
                {
                    var author = await _repo.FindUser(x.AuthorId);

                    if (author != null)
                    {
                        x.AuthorName = author.Username;
                    }

                    return x;
                }));
        }

        public async Task AddCroakAsync(CroakDto croakDto)
        {
            var croak = _mapper.Map<Croak>(croakDto);
            var croakId = await _repo.AddCroak(croak);

            var hashtags = croak.Hashtags.Select(x => new Hashtag()
            {
                Caption = x,
                CroakIds = new List<int>() { croakId }
            });

            foreach (var ht in hashtags)
            {
                var existingHt = await _repo.FindHashtag(ht.Caption);

                if (existingHt == null)
                {
                    await _repo.AddHashtag(ht);
                }
                else 
                {
                    existingHt.CroakIds.AddRange(ht.CroakIds);
                    await _repo.UpdateHashtag(existingHt);
                }
            }
            
            await NotifyOnChange?.Invoke();
        }

        public async Task RemoveCroakAsync(int id)
        {
            var croak = await _repo.FindCroak(id);

            if (croak == null)
            {
                return;
            }

            await RemoveCroakLikesAsync(croak.Id);
            await RemoveCroakRefsFromHashtagsAsync(croak.Hashtags, croak.Id);
            await _repo.RemoveCroak(id);
            await NotifyOnChange?.Invoke();
        }

        protected async Task RemoveCroakLikesAsync(int croakId)
        {
            var likes = await _repo.FindLikes(croakId);

            foreach (var like in likes)
            {
                await _repo.RemoveLike(like);
            }
        }

        protected async Task RemoveCroakRefsFromHashtagsAsync(IEnumerable<string> hashtagCaptions, int croakId)
        {
            var hashtags = await _repo.FindHashtags(hashtagCaptions);
            
            foreach (var ht in hashtags)
            {
                ht.CroakIds.Remove(croakId);

                if (ht.CroakIds.Count == 0)
                {
                    await _repo.RemoveHashtag(ht.Id);
                }
                else 
                {
                    await _repo.UpdateHashtag(ht);
                }
            }
        }

        public async Task<bool> LikeCroakAsync(LikeDto likeDto)
        {
            
            var user = await _repo.FindUser(likeDto.UserId);

            if (user == null)
            {
                return false;
            }

            if (!await TryFindAndUpdateCroak(likeDto.CroakId, (croak) => croak.Likes++))
            {
                return false;
            }

            var like = _mapper.Map<Like>(likeDto);
            return await _repo.AddLike(like) > 0;
        }

        public async Task<bool> UnlikeCroakAsync(LikeDto likeDto)
        {
            var like = await _repo.FindLike(likeDto.UserId, likeDto.CroakId);

            if (like == null)
            {
                return false;
            }

            if (!await TryFindAndUpdateCroak(likeDto.CroakId, (croak) => croak.Likes--))
            {
                return false;
            }

            return await _repo.RemoveLike(like);
        }

        protected async Task<bool> TryFindAndUpdateCroak(int croakId, Action<Croak> updater)
        {
            var croak = await _repo.FindCroak(croakId);

            if (croak == null)
            {
                return false;
            }

            updater(croak);
            return await _repo.UpdateCroak(croak);
        }

        public async Task<bool> IsLiked(LikeDto likeDto)
        {
            return await _repo.FindLike(likeDto.UserId, likeDto.CroakId) != null;
        }

        public async Task<IEnumerable<HashtagPopularity>> GetPopularHastags()
        {
            var hashtags = await _repo.GetHashtagPopularities(MAX_POPULAR_HASHTAGS);
            return hashtags;
        }

        public event Func<Task> NotifyOnChange;
    }
}