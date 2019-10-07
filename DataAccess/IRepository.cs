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

        Task<int> AddHashtag(Hashtag hashtag);

        Task<IEnumerable<int>> GetCroakIdsWithHashtag(string hashtag);

        Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities();
    }
}