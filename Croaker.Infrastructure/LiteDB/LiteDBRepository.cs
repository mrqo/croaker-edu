using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.LiteDB.Data;
using LiteDB;

using edu_croaker.Data.Entities;
using edu_croaker.Data.Interfaces;

namespace edu_croaker.Infrastructure.LiteDB
{
    public class LiteDBRepository<T> : IRepository<T> where T : EntityBase
    {
        protected LiteCollection<T> Collection { get; private set; }

        public LiteDBRepository(ILiteDbContext ctx)
        {
            Collection = ctx.LiteDatabase.GetCollection<T>();
        }

        public virtual T Get(ISpecification<T> predicate)
        {
            return Collection.FindOne(predicate.Criteria);
        }

        public virtual T GetById(int id)
        {
            return Collection.FindById(id);
        }

        public virtual IEnumerable<T> List()
        {
            return Collection.FindAll();
        }

        public IEnumerable<T> List(ISpecification<T> predicate)
        {
            return Collection.Find(predicate.Criteria);
        }

        public virtual int Add(T entity)
        {
            return (int)Collection.Insert(entity);
        }

        public virtual void Delete(T entity)
        {
            Collection.Delete(new BsonValue(entity.Id));
        }

        public virtual void Edit(T entity)
        {
            Collection.Update(entity);
        }
    }
}
