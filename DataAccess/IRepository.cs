using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using edu_croaker.Data;
using edu_croaker.Dtos;

namespace edu_croaker.DataAccess
{
    public interface IRepository
    {
        Task<int> AddCroak(Croak croak);

        Task<Croak> FindCroak(int id);

        Task<IEnumerable<Croak>> FindCroaks();

        Task<IEnumerable<Croak>> FindCroaks(IEnumerable<int> ids);

        Task<bool> RemoveCroak(int id);

        Task<int> AddHashtag(Hashtag hashtag);

        Task<Hashtag> FindHashtag(string caption);

        Task<IEnumerable<Hashtag>> FindHashtags(IEnumerable<string> captions);

        Task<bool> UpdateHashtag(Hashtag ht);
        
        Task<bool> RemoveHashtag(int id);
        
        Task<IEnumerable<int>> FindCroakIdsWithHashtag(int id);

        Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities(int maxCount);
    }
}