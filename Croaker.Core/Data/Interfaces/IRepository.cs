using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        T Get(ISpecification<T> predicate);

        T GetById(int id);

        IEnumerable<T> List();

        IEnumerable<T> List(ISpecification<T> predicate);

        int Add(T entity);

        void Delete(T entity);

        void Edit(T entity);
    }
}