using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace edu_croaker.Data
{
    public class CroakService
    {
        private List<Croak> Croaks = new List<Croak> 
        {
            new Croak
            {
                Title = "Aliens are cool",
                Author = "Marek",
                Content = "Aliens are not that bad, I really like them #alike #aliens",
                Shares = 5,
            },
            new Croak
            {
                Title = "Hello world!",
                Author = "Marek",
                Content = "It's my first croak!",
                Shares = 2,
            },
        };

        public Task<IEnumerable<Croak>> GetCroaksAsync()
        {
            // Simulate loading from database.
            return Task.Run(() => Croaks.AsEnumerable().Reverse());
        }

        public async Task AddCroakAsync(Croak croak)
        {
            Croaks.Add(croak);
            
            if (NotifyOnChange != null)
            {
                await NotifyOnChange.Invoke();
            }
        }

        public event Func<Task> NotifyOnChange;
    }
}