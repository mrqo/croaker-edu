using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LiteDB;
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

        public Repository()
        {
            Db = new LiteDatabase(DB_PATH);
            Croaks = Db.GetCollection<Croak>("croaks");
            Hashtags = Db.GetCollection<Hashtag>("hashtags");
        }

        public async Task AddCroak(Croak croak)
        {
            await Task.Run(() => Croaks.Insert(croak));
        }

        public async Task<bool> RemoveCroak(int id)
        {
            return await Task.Run(() => Croaks.Delete(new LiteDB.BsonValue(id)));
        }

        public async Task AddHashtag(Hashtag hashtag)
        {
            await Task.Run(() => Hashtags.Insert(hashtag));
        }

        public async Task<IEnumerable<int>> GetCroakIdsWithHashtag(string caption)
        {
            return await Task.Run(() => Hashtags
                .FindOne(Query.EQ("Caption", caption))
                .CroakIds
            );
        }

        public async Task<IEnumerable<Croak>> GetAllCroaks()
        {
            return Croaks.FindAll();
        }

        public async Task<IEnumerable<HashtagPopularity>> GetHashtagPopularities()
        {
            return await Task.Run(() => Hashtags
                .FindAll()
                .OrderByDescending(x => x.CroakIds.Count)
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