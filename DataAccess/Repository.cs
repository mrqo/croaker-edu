using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;
using edu_croaker.Data;
using edu_croaker.Dtos;


namespace edu_croaker.DataAccess
{
    public class Repository : IRepository, IDisposable
    {
        public static string DB_PATH = @"Croak.db";

        protected readonly IMapper _mapper;
        protected LiteDatabase Db { get; private set; }
        protected LiteCollection<Croak> Croaks { get; private set; }
        protected LiteCollection<Hashtag> Hashtags { get; private set; }
        protected LiteCollection<ApplicationUser> Users { get; private set; }
        protected LiteCollection<Follower> Followers { get; private set; }

        public Repository(ILiteDbContext ctx, IMapper mapper)
        {
            _mapper = mapper;
            Db = ctx.LiteDatabase;
            Croaks = Db.GetCollection<Croak>("croaks");
            Hashtags = Db.GetCollection<Hashtag>("hashtags");
            Users = Db.GetCollection<ApplicationUser>("users");
            Followers = Db.GetCollection<Follower>("followers");
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
            return Task.Run(() =>
            {
                var croaks = Croaks.FindAll();
                return croaks.Where(croak => ids.Contains(croak.Id));
            });
        }
        
        public Task<IEnumerable<Croak>> FindCroaksByAuthor(string authorName)
        {
            return Task.Run(() =>
            {
                var authorId = Users.FindOne(Query.EQ("UserName", authorName))?.Id ?? "";
                return Croaks.Find(Query.EQ("Author", authorId));
            });
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

        public Task<PublicUserData> FindUser(string id)
        {
            return Task.Run(() =>
            {
                var appUser = Users.FindById(id);
                return _mapper.Map<PublicUserData>(appUser);
            });
        }

        public Task<int> AddFollower(Follower follower)
        {
            return Task.Run(() => (int)Followers.Insert(follower));
        }

        public Task<bool> RemoveFollower(Follower follower)
        {
            return Task.Run(() => Followers.Delete(x => x.Equals(follower)) > 0);
        }

        public void Dispose()
        {
            Db.Dispose();
        } 
    }
}