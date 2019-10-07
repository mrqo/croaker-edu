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
            var croaks = await Repo.GetAllCroaks();
            return croaks.Reverse();
        }

        public async Task<IEnumerable<Croak>> GetCroaksWithHashtagAsync(int hashtagId)
        {
            var ids = await Repo.GetCroakIdsWithHashtag(hashtagId);
            return await Repo.GetCroaks(ids);
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
                // #TODO: Check if exists and update / add respectively.
                await Repo.AddHashtag(ht);
            }
            

            if (NotifyOnChange != null)
            {
                await NotifyOnChange.Invoke();
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