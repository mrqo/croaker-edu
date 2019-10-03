using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using edu_croaker.DataAccess;

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

        public async Task AddCroakAsync(Croak croak)
        {
            await Repo.AddCroak(croak);

            if (NotifyOnChange != null)
            {
                await NotifyOnChange.Invoke();
            }
        }

        public event Func<Task> NotifyOnChange;
    }
}