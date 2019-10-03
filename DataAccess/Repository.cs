using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;
using edu_croaker.Data;

namespace edu_croaker.DataAccess
{
    public class Repository : IRepository, IDisposable
    {
        public static string DB_PATH = @"Croak.db";

        protected LiteDatabase Db { get; private set; }
        protected LiteCollection<Croak> Croaks { get; private set; }

        public Repository()
        {
            Db = new LiteDatabase(DB_PATH);
            Croaks = Db.GetCollection<Croak>("croaks");
        }

        public async Task AddCroak(Croak croak)
        {
            await Task.Run(() => Croaks.Insert(croak));
        }

        public async Task<bool> RemoveCroak(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddHashtag(string hashtag)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<int>> GetCroakIdsWithHashtag(string hashtag)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Croak>> GetAllCroaks()
        {
            return Croaks.FindAll();
        }

        public void Dispose()
        {
            Db.Dispose();
        } 
    }
}