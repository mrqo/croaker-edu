using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using edu_croaker.Data;

namespace edu_croaker.DataAccess
{
    public interface IRepository
    {
        Task AddCroak(Croak croak);

        Task<IEnumerable<Croak>> GetAllCroaks();
    }
}