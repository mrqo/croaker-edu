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

        Task<bool> RemoveCroak(int id);

        Task<IEnumerable<Croak>> GetAllCroaks();

        Task<IEnumerable<Croak>> GetCroaks(IEnumerable<int> ids);
        
        Task<int> AddHashtag(Hashtag hashtag);

        Task<IEnumerable<int>> GetCroakIdsWithHashtag(int id);

        Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities();
    }
}