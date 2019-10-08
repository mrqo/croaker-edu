using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using edu_croaker.DataAccess;
using edu_croaker.Dtos;

namespace edu_croaker.Data
{
    public class CroakService
    {
        protected readonly IRepository Repo;

        public CroakService(IRepository repo)
        {
            Repo = repo;
        }

        public async Task<IEnumerable<Croak>> GetCroaksAsync()
        {
            var croaks = await Repo.FindCroaks();
            return croaks.Reverse();
        }

        public async Task<IEnumerable<Croak>> GetCroaksWithHashtagAsync(int hashtagId)
        {
            var ids = await Repo.FindCroakIdsWithHashtag(hashtagId);
            return await Repo.FindCroaks(ids);
        }

        public async Task AddCroakAsync(Croak croak)
        {
            var croakId = await Repo.AddCroak(croak);

            var hashtags = croak.Hashtags.Select(x => new Hashtag()
            {
                Caption = x,
                CroakIds = new List<int>() { croakId }
            });

            foreach (var ht in hashtags)
            {
                var existingHt = await Repo.FindHashtag(ht.Caption);

                if (existingHt == null)
                {
                    await Repo.AddHashtag(ht);
                }
                else 
                {
                    existingHt.CroakIds.AddRange(ht.CroakIds);
                    await Repo.UpdateHashtag(existingHt);
                }
            }
            
            await NotifyOnChange?.Invoke();
        }

        public async Task RemoveCroakAsync(int id)
        {
            var croak = await Repo.FindCroak(id);

            if (croak == null)
            {
                return;
            }

            await RemoveCroakRefsFromHashtagsAsync(croak.Hashtags, croak.Id);
            await Repo.RemoveCroak(id);
        }

        protected async Task RemoveCroakRefsFromHashtagsAsync(IEnumerable<string> hashtagCaptions, int croakId)
        {
            var hashtags = await Repo.FindHashtags(hashtagCaptions);
            
            foreach (var ht in hashtags)
            {
                ht.CroakIds.Remove(croakId);
                await Repo.UpdateHashtag(ht);
            }
        }

        public async Task<IEnumerable<HashtagPopularity>> GetPopularHastags()
        {
            var hashtags = await Repo.GetHashtagPopularities();
            return hashtags;
        }

        public event Func<Task> NotifyOnChange;
    }
}