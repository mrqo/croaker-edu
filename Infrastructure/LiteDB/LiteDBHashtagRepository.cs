using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.LiteDB.Data;

using edu_croaker.Data.Dtos;
using edu_croaker.Data.Entities;
using edu_croaker.Data.Interfaces;

namespace edu_croaker.Infrastructure.LiteDB
{
    public class LiteDBHashtagRepository : LiteDBRepository<Hashtag>, IHashtagRepository
    {
        public LiteDBHashtagRepository(ILiteDbContext ctx)
            : base(ctx)
        {
        }

        public virtual IEnumerable<HashtagPopularity> ListPopular(int maxCount)
        {
            // #TODO: Implement it.
            return Collection.FindAll()
                .OrderByDescending(x => x.CroakIds.Count)
                .Take(maxCount)
                .Select(x => new HashtagPopularity()
                {
                    Hashtag = new Hashtag()
                    {
                        Id = x.Id,
                        Caption = x.Caption
                    },
                    HitCount = x.CroakIds.Count
                });
        }
    }
}
