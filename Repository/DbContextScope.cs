using DatabaseContext;
using System;
using System.Data.Entity;

namespace Repository
{
    public class DbContextScope : IDisposable
    {
        public static DbContext DbContext { get; set; }

        public DbContextScope()
        {
            DbContext = new DemoContext();
        }

        public Database Database()
        {
            return DbContext.Database;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
