using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Model.Repository
{
    public class Repository<T> where T : class, new()
    {
        private DemoContext DatabaseContext = null;
        protected DbSet<T> DbSet = null;

        #region Basic CRUD Operations
        protected T Get(Expression<Func<T, bool>> condition = null)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query.FirstOrDefault();
        }

        protected T Get(Expression<Func<T, bool>> condition = null, params Expression<Func<T, dynamic>>[] includes)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query.FirstOrDefault();
        }

        protected IQueryable<T> GetAll()
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            return query;
        }

        protected IQueryable<T> GetAll(params Expression<Func<T, dynamic>>[] includes)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        protected IQueryable<T> FindBy(Expression<Func<T, bool>> condition)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query;
        }

        protected IQueryable<T> FindBy(Expression<Func<T, bool>> condition, params Expression<Func<T, dynamic>>[] includes)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query;
        }

        protected void Insert(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Added;
        }

        protected void Insert(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DatabaseContext.Entry(e).State = EntityState.Added;
            }
        }

        protected void Update(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Modified;
        }

        protected void Update(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DatabaseContext.Entry(e).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Update or Insert an entity by partial fields, is an experimental code ..
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        protected void PartialUpdateOrInsert<TProperty>(T t, params Expression<Func<T, TProperty>>[] properties)
        {
            DatabaseContext.Configuration.AutoDetectChangesEnabled = false;
            DatabaseContext.Configuration.ValidateOnSaveEnabled = false;

            DbSet.Attach(t);
            var entry = DatabaseContext.Entry(t);

            foreach (var p in properties) entry.Property(p).IsModified = true;
        }

        protected void Delete(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Deleted;
        }

        protected void Save()
        {
            DatabaseContext.SaveChanges();
        }
        #endregion

        #region Sql Query
        protected IEnumerable<T> SqlQuery(string query, params dynamic[] parameters)
        {
            return DatabaseContext.Database.SqlQuery<T>(query, parameters);
        }

        protected void ExecuteSqlCommand(string query, params dynamic[] parameters)
        {
            DatabaseContext.Database.ExecuteSqlCommand(query, parameters);
        }
        #endregion

        #region Transaction
        public DbContextTransaction BeginsTransaction()
        {
            return DatabaseContext.Database.BeginTransaction();
        }
        #endregion

        public DemoContext ContextScope(bool LazyLoadingEnabled = false, bool ProxyCreationEnabled = false)
        {
            DatabaseContext = new DemoContext();
            DbSet = DatabaseContext.Set<T>();

            DatabaseContext.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            DatabaseContext.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;

            return DatabaseContext;
        }
    }
}
