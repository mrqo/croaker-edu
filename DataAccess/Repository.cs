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

        public async Task<int> AddCroak(Croak croak)
        {
            return await Task.Run(() => Croaks.Insert(croak));
        }

        public async Task<bool> RemoveCroak(int id)
        {
            return await Task.Run(() => Croaks.Delete(new LiteDB.BsonValue(id)));
        }

        public async Task<int> AddHashtag(Hashtag hashtag)
        {
            return await Task.Run(() => Hashtags.Insert(hashtag));
        }

        public async Task<IEnumerable<int>> GetCroakIdsWithHashtag(int id)
        {
            return await Task.Run(() => {
                var hashtag = Hashtags.FindOne(Query.EQ("Id", id));

                if (hashtag != null)
                    return hashtag.CroakIds;
                
                return new List<int>();
            });
        }

        public async Task<IEnumerable<Croak>> GetAllCroaks()
        {
            return Croaks.FindAll();
        }

        public async Task<IEnumerable<Croak>> GetCroaks(IEnumerable<int> ids)
        {
            return await Task.Run(() => Croaks.Find(x => ids.Contains(x.Id)));
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