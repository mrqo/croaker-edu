using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

using edu_croaker.Data.Dtos;
using edu_croaker.Data.Entities;
using edu_croaker.Data.Interfaces;
using edu_croaker.Data.Specifications;

using AspNetCore.Identity.LiteDB.Models;

namespace edu_croaker.Services
{
    public class CroakService
    {
        private readonly IRepository<Croak> _croaksRepo;
        private readonly IRepository<Like> _likesRepo;
        private readonly IRepository<Hashtag> _hashtagsRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public const int MAX_POPULAR_HASHTAGS = 5;

        public CroakService(
            IRepository<Croak> croaksRepo,
            IRepository<Like> likesRepo,
            IRepository<Hashtag> hashtagsRepo,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
        )
        {
            _croaksRepo = croaksRepo;
            _likesRepo = likesRepo;
            _hashtagsRepo = hashtagsRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksAsync()
        {
            var croaks = _croaksRepo.List();

            var croakDtos = croaks
                .Select(x => _mapper.Map<CroakDto>(x))
                .Reverse();

            return await GetWithUserNamesAsync(croakDtos);
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksWithHashtagAsync(string caption)
        {
            var hashtag = _hashtagsRepo.Get(new HashtagsByCaptionSpecification(caption));
            var croakIds = hashtag?.CroakIds;
            var croaks = croakIds.Select(id => _croaksRepo.GetById(id));
 
            var croakDtos = croaks
                .Select(x => _mapper.Map<CroakDto>(x))
                .Reverse();

            return await GetWithUserNamesAsync(croakDtos);
        }

        public async Task<IEnumerable<CroakDto>> GetCroaksByAuthorAsync(string authorName)
        {
            var user = await _userManager.FindByNameAsync(authorName);
            var croaks = _croaksRepo.List(new CroaksByAuthorSpecification(user?.Id));

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
                    var author = await _userManager.FindByIdAsync(x.AuthorId);

                    if (author != null)
                    {
                        x.AuthorName = author.UserName;
                    }

                    return x;
                }));
        }

        public void AddCroakAsync(CroakDto croakDto)
        {
            var croak = _mapper.Map<Croak>(croakDto);
            var croakId = _croaksRepo.Add(croak);

            var hashtags = croak.Hashtags.Select(x => new Hashtag()
            {
                Caption = x,
                CroakIds = new List<int>() { croakId }
            });

            foreach (var ht in hashtags)
            {
                var existingHt = _hashtagsRepo.Get(new HashtagsByCaptionSpecification(ht.Caption));

                if (existingHt == null)
                {
                    _hashtagsRepo.Add(ht);
                }
                else 
                {
                    existingHt.CroakIds.AddRange(ht.CroakIds);
                    _hashtagsRepo.Edit(existingHt);
                }
            }
            
            NotifyOnChange?.Invoke();
        }

        public void RemoveCroak(int id)
        {
            var croak = _croaksRepo.GetById(id);

            if (croak == null)
            {
                return;
            }

            RemoveCroakLikes(croak.Id);
            RemoveCroakRefsFromHashtags(croak.Hashtags, croak.Id);
            _croaksRepo.Delete(croak);
            NotifyOnChange?.Invoke();
        }

        protected void RemoveCroakLikes(int croakId)
        {
            var likes = _likesRepo.List(new LikesByCroakSpecification(croakId));

            foreach (var like in likes)
            {
                _likesRepo.Delete(like);
            }
        }

        protected void RemoveCroakRefsFromHashtags(IEnumerable<string> hashtagCaptions, int croakId)
        {
            var hashtags = _hashtagsRepo.List(new HashtagsByCaptionsSpecification(hashtagCaptions));
            
            foreach (var ht in hashtags)
            {
                ht.CroakIds.Remove(croakId);

                if (ht.CroakIds.Count == 0)
                {
                    _hashtagsRepo.Delete(ht);
                }
                else 
                {
                    _hashtagsRepo.Edit(ht);
                }
            }
        }

        public async Task<bool> LikeCroakAsync(LikeDto likeDto)
        {
            var user = await _userManager.FindByIdAsync(likeDto.UserId);

            if (user == null)
            {
                return false;
            }

            if (!TryFindAndUpdateCroak(likeDto.CroakId, (croak) => croak.Likes++))
            {
                return false;
            }

            var like = _mapper.Map<Like>(likeDto);
            return _likesRepo.Add(like) > 0;
        }

        public bool UnlikeCroak(LikeDto likeDto)
        {
            var like = _likesRepo.Get(new LikesByUserAndCroakSpecification(likeDto.UserId, likeDto.CroakId));

            if (like == null)
            {
                return false;
            }

            if (!TryFindAndUpdateCroak(likeDto.CroakId, (croak) => croak.Likes--))
            {
                return false;
            }

            _likesRepo.Delete(like);
            return true;
        }

        protected bool TryFindAndUpdateCroak(int croakId, Action<Croak> updater)
        {
            var croak = _croaksRepo.GetById(croakId);

            if (croak == null)
            {
                return false;
            }

            updater(croak);
            _croaksRepo.Edit(croak);
            return true;
        }

        public bool IsLiked(LikeDto likeDto)
        {
            return _likesRepo.Get(new LikesByUserAndCroakSpecification(likeDto.UserId, likeDto.CroakId)) != null;
        }

        public IEnumerable<HashtagPopularityDto> GetPopularHastags()
        {
            return (_hashtagsRepo as IHashtagRepository).ListPopular(MAX_POPULAR_HASHTAGS)
                .Select(ht => _mapper.Map<HashtagPopularityDto>(ht));
        }

        public event Func<Task> NotifyOnChange;
    }
}