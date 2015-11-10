using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using Repository.Interfaces;

namespace Repository
{
    public class Repository<T> where T : class, new()
    {
        private DbContext DatabaseContext;
        public DbSet<T> DbSet;

        #region Basic CRUD Operations

        /// <summary>
        /// Obtiene una entidad de nuestra base datos.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> condition = null)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene una entidad de nuestra base datos incluyendo sus asociaciones.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> condition = null, params Expression<Func<T, dynamic>>[] includes)
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

        /// <summary>
        /// Retorna un objeto Queryable, si queremos que traiga información de la base de datos podemos usar un GetAll().ToList()
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            return query;
        }


        /// <summary>
        /// Retorna un objeto Queryable, si queremos que traiga información de la base de datos podemos usar un GetAll().ToList().
        /// Adicionalmente, podemos especificar que asocaciones queremos que incluya.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(params Expression<Func<T, dynamic>>[] includes)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        /// <summary>
        /// Retorna un objeto Queryable especificado por una condición
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> condition)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            query = query.Where(condition);

            return query;
        }

        /// <summary>
        /// Retorna un objeto Queryable especificado por una condición y podemos especificar que asociaciones podemos incluir.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<T> FindBy(Expression<Func<T, bool>> condition, params Expression<Func<T, dynamic>>[] includes)
        {
            var query = DatabaseContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            query = query.Where(condition);

            return query;
        }

        /// <summary>
        /// Inserta un registro en nuestra tabla
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Added;
        }

        /// <summary>
        /// Inserta varios registros en nuestra tabla
        /// </summary>
        /// <param name="t"></param>
        public void Insert(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DatabaseContext.Entry(e).State = EntityState.Added;
            }
        }

        /// <summary>
        /// Actualiza un registro de nuestra tabla
        /// </summary>
        /// <param name="t"></param>
        public void Update(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Modified;
        }

        /// <summary>
        /// Actualiza varios registros de nuestra tabla
        /// </summary>
        /// <param name="t"></param>
        public void Update(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                DatabaseContext.Entry(e).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Actualiza unicamente las columnas/propiedades que le indiquemos en el segundo parámetro Ejm: x => x.Nombre, x => x.Apellido
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        public void PartialUpdate<TProperty>(T t, params Expression<Func<T, TProperty>>[] properties)
        {
            DatabaseContext.Configuration.AutoDetectChangesEnabled = false;
            DatabaseContext.Configuration.ValidateOnSaveEnabled = false;

            DbSet.Attach(t);
            var entry = DatabaseContext.Entry(t);

            foreach (var p in properties) entry.Property(p).IsModified = true;
        }

        /// <summary>
        /// Elimina un registro de nuestra tabla
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fields"></param>
        public void Delete(T t)
        {
            DatabaseContext.Entry(t).State = EntityState.Deleted;
        }

        public void Save()
        {
            DatabaseContext.SaveChanges();
        }
        #endregion

        #region Sql Query
        /// <summary>
        /// Ejecuta un query personalizado de SQL, la segunda opción son los parámetros a limitar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery(string query, params dynamic[] parameters)
        {
            return DatabaseContext.Database.SqlQuery<T>(query, parameters);
        }

        /// <summary>
        /// Ejecuta un query personalizado de SQL, la segunda opción son los parámetros a limitar. Este método es solo de entrada, no se espera un retorno.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void ExecuteSqlCommand(string query, params dynamic[] parameters)
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

        public DbContext ContextScope(DbContext context, bool LazyLoadingEnabled = false, bool ProxyCreationEnabled = false)
        {
            DatabaseContext = context;
            DbSet = DatabaseContext.Set<T>();

            DatabaseContext.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            DatabaseContext.Configuration.ProxyCreationEnabled = ProxyCreationEnabled;

            return DatabaseContext;
        }
    }
}
