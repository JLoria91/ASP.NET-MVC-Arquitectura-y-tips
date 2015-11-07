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
        protected T Get(params Expression<Func<T, bool>>[] conditions)
        {
            var query = DatabaseContext.Set<T>();

            foreach (var c in conditions) query.Where(c);

            return query.FirstOrDefault();
        }

        protected IEnumerable<T> GetAll()
        {
            return DatabaseContext.Set<T>();
        }

        protected IEnumerable<T> FindBy(params Expression<Func<T, bool>>[] conditions)
        {
            var query = DatabaseContext.Set<T>();

            foreach (var c in conditions) query.Where(c);

            return query;
        }

        protected void Insert(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Added;
        }

        protected void Update(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Modified;
        }

        /// <summary>
        /// Update a entity by partial fields, is an experimental code ..
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        protected void PartialUpdate<TProperty>(T t, params Expression<Func<T, TProperty>>[] properties)
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

        public DemoContext ContextScope()
        {
            DatabaseContext = new DemoContext();
            DbSet = DatabaseContext.Set<T>();

            return DatabaseContext;
        }
    }
}
