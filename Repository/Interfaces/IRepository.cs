using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);

        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);

        void Insert(IEnumerable<T> entities);
        void Update(IEnumerable<T> entities);

        void PartialUpdate(T t, params Expression<Func<T, object>>[] properties);
        IEnumerable<T> SqlQuery(string query, params dynamic[] parameters);
        int ExecuteSqlCommand(string query, params dynamic[] parameters);
    }
}
