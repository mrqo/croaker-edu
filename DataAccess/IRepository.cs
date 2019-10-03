using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using edu_croaker.Data;

namespace edu_croaker.DataAccess
{
    public interface IRepository
    {
        Task AddCroak(Croak croak);

        Task<bool> RemoveCroak(int id);

        Task<IEnumerable<Croak>> GetAllCroaks();

        Task AddHashtag(string hashtag);

        Task<IEnumerable<int>> GetCroakIdsWithHashtag(string hashtag);
    }
}