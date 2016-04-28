using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using Repository.Interfaces;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>> includeProperties,
                                                       IQueryable<T> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        #region IRepository<T> Members
        public IQueryable<T> AsQueryable()
        {
            return DbContextScope.DbContext.Set<T>().AsQueryable();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            return PerformInclusions(includeProperties, query);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = AsQueryable();
            return query;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.FirstOrDefault(where);
        }

        public void Delete(T entity)
        {
            DbContextScope.DbContext.Set<T>().Remove(entity);
        }

        public void Insert(T entity)
        {
            DbContextScope.DbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            DbContextScope.DbContext.Set<T>().Attach(entity);
            DbContextScope.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Insert(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DbContextScope.DbContext.Entry(e).State = EntityState.Added;
            }
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DbContextScope.DbContext.Entry(e).State = EntityState.Modified;
            }
        }

        public void PartialUpdate(T t, params Expression<Func<T, object>>[] properties)
        {
            var entry = DbContextScope.DbContext.Entry(t);
            foreach (var p in properties) entry.Property(p).IsModified = true;
        }
        #endregion

        public IEnumerable<T> SqlQuery(string query, params dynamic[] parameters)
        {
            return DbContextScope.DbContext.Database.SqlQuery<T>(query, parameters);
        }

        public int ExecuteSqlCommand(string query, params dynamic[] parameters)
        {
            return DbContextScope.DbContext.Database.ExecuteSqlCommand(query, parameters);
        }

        //private DbContext DatabaseContext;
        //public DbSet<T> DbSet;

        //public T Get(Expression<Func<T, bool>> condition = null)
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    if (condition != null)
        //    {
        //        query = query.Where(condition);
        //    }

        //    return query.FirstOrDefault();
        //}
        //public T Get(Expression<Func<T, bool>> condition = null, params Expression<Func<T, dynamic>>[] includes)
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    if (condition != null)
        //    {
        //        query = query.Where(condition);
        //    }

        //    return query.FirstOrDefault();
        //}
        //public IQueryable<T> GetAll()
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    return query;
        //}
        //public IQueryable<T> GetAll(params Expression<Func<T, dynamic>>[] includes)
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    return query;
        //}
        //public IQueryable<T> FindBy(Expression<Func<T, bool>> condition)
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    query = query.Where(condition);

        //    return query;
        //}
        //public IQueryable<T> FindBy(Expression<Func<T, bool>> condition, params Expression<Func<T, dynamic>>[] includes)
        //{
        //    var query = DatabaseContext.Set<T>().AsQueryable();

        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    query = query.Where(condition);

        //    return query;
        //}
        //public void Insert(T t)
        //{
        //    DatabaseContext.Entry(t).State = EntityState.Added;
        //}
        //public void Insert(IEnumerable<T> entities)
        //{
        //    foreach (var e in entities)
        //    {
        //        DatabaseContext.Entry(e).State = EntityState.Added;
        //    }
        //}
        //public void Update(T t)
        //{
        //    DatabaseContext.Entry(t).State = EntityState.Modified;
        //}
        //public void Update(IEnumerable<T> entities)
        //{
        //    foreach (var e in entities)
        //    {
        //        DatabaseContext.Entry(e).State = EntityState.Modified;
        //    }
        //}
        //public void PartialUpdate(T t, params Expression<Func<T, object>>[] properties)
        //{
        //    DatabaseContext.Configuration.AutoDetectChangesEnabled = false;
        //    DatabaseContext.Configuration.ValidateOnSaveEnabled = false;

        //    DbSet.Attach(t);
        //    var entry = DatabaseContext.Entry(t);

        //    foreach (var p in properties) entry.Property(p).IsModified = true;
        //}
        //public void Delete(T t)
        //{
        //    DatabaseContext.Entry(t).State = EntityState.Deleted;
        //}

        //public void Save()
        //{
        //    DatabaseContext.SaveChanges();
        //}

        //public IEnumerable<T> SqlQuery(string query, params dynamic[] parameters)
        //{
        //    return DatabaseContext.Database.SqlQuery<T>(query, parameters);
        //}

        //public void ExecuteSqlCommand(string query, params dynamic[] parameters)
        //{
        //    DatabaseContext.Database.ExecuteSqlCommand(query, parameters);
        //}

        //public DbContextTransaction BeginsTransaction()
        //{
        //    return DatabaseContext.Database.BeginTransaction();
        //}

        //public DbContext ContextScope(DbContext context, bool LazyLoadingEnabled = false, bool ProxyCreationEnabled = false)
        //{
        //    DatabaseContext = context;
        //    DbSet = DatabaseContext.Set<T>();

        //    DatabaseContext.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
        //    DatabaseContext.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;

        //    return DatabaseContext;
        //}

        //public DbContext ContextScope(DbContext context, bool LazyLoadingEnabled = false, bool ProxyCreationEnabled = false)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
