using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using edu_croaker.Data;
using edu_croaker.Dtos;

namespace edu_croaker.DataAccess
{
    public class Repository : IRepository, IDisposable
    {
        public static string DB_PATH = @"Croak.db";

        protected LiteDatabase Db { get; private set; }
        protected LiteCollection<Croak> Croaks { get; private set; }
        protected LiteCollection<Hashtag> Hashtags { get; private set; }

        public Repository(ILiteDbContext ctx)
        {
            Db = ctx.LiteDatabase;
            Croaks = Db.GetCollection<Croak>("croaks");
            Hashtags = Db.GetCollection<Hashtag>("hashtags");
        }

        public Task<int> AddCroak(Croak croak)
        {
            return Task.Run(() => (int)Croaks.Insert(croak));
        }
        
        public Task<Croak> FindCroak(int id)
        {
            return Task.Run(() => Croaks.FindById(id));
        }

        public Task<IEnumerable<Croak>> FindCroaks()
        {
            return Task.Run(() => Croaks.FindAll());
        }

        public Task<IEnumerable<Croak>> FindCroaks(IEnumerable<int> ids)
        {
            return Task.Run(() => Croaks.Find(x => ids.Contains(x.Id)));
        }

        public Task<bool> RemoveCroak(int id)
        {
            return Task.Run(() => 
            {
                return Croaks.Delete(new LiteDB.BsonValue(id));
            });
        }

        public Task<int> AddHashtag(Hashtag hashtag)
        {
            return Task.Run(() => 
            {
                return (int)Hashtags.Insert(hashtag);
            });
        }

        public Task<Hashtag> FindHashtag(string caption)
        {
            return Task.Run(() => Hashtags
                .FindOne(Query.EQ("Caption", caption))
            );
        }

        public Task<IEnumerable<Hashtag>> FindHashtags(IEnumerable<string> captions)
        {
            return Task.Run(() => 
            {
                var bsonCaptions = captions.Select(x => new BsonValue(x));

                return Hashtags
                    .Find(Query.In("Caption", bsonCaptions));
            });
        }

        public Task<bool> UpdateHashtag(Hashtag ht)
        {
            return Task.Run(() => Hashtags.Update(ht));
        }

        public Task<bool> RemoveHashtag(int id)
        {
            return Task.Run(() => Hashtags.Delete(id));
        }
        
        public Task<IEnumerable<int>> FindCroakIdsWithHashtag(int id)
        {
            return Task.Run(() => {
                var hashtag = Hashtags.FindById(id);

                if (hashtag != null)
                    return hashtag.CroakIds;
                
                return new List<int>().AsEnumerable();
            });
        }

        public Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities(int maxCount)
        {
            return Task.Run(() => Hashtags
                .FindAll()
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
                })
            );
        }

        public void Dispose()
        {
            Db.Dispose();
        } 
    }
}