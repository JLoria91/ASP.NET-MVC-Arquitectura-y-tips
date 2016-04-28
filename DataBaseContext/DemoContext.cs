namespace DatabaseContext
{
    using System.Data.Entity;
    using System.Linq;
    using System;
    using Model;
    using Model.Interfaces;
    using Common;
    public partial class DemoContext : DbContext
    {
        public DemoContext()
            : base("name=DemoContext")
        {
        }

        public virtual DbSet<Adjunto> Adjunto { get; set; }
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<AlumnoCurso> AlumnoCurso { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adjunto>()
                .Property(e => e.Archivo)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.FechaNacimiento)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.Adjunto)
                .WithRequired(e => e.Alumno)
                .HasForeignKey(e => e.Alumno_id);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.AlumnoCurso)
                .WithRequired(e => e.Alumno)
                .HasForeignKey(e => e.Alumno_id);

            modelBuilder.Entity<Curso>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.AlumnoCurso)
                .WithRequired(e => e.Curso)
                .HasForeignKey(e => e.Curso_id);

            modelBuilder.Entity<Pais>()
                .HasMany(e => e.Alumnos)
                .WithOptional(e => e.Pais)
                .HasForeignKey(e => e.Pais_id);
        }

        public override int SaveChanges()
        {
            Auditar();
            return base.SaveChanges();
        }

        public void Auditar()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(
                x => x.Entity is IAuditable && (x.State == EntityState.Added
                     || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditable entity = entry.Entity as IAuditable;
                if (entity != null)
                {
                    var fecha = DateTime.Now;
                    var usuario = SessionHelper.GetUser();

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreadoFecha = fecha;
                        entity.CreadoPor = usuario;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreadoFecha).IsModified = false;
                        base.Entry(entity).Property(x => x.CreadoPor).IsModified = false;
                    }

                    entity.ActualizadoFecha = fecha;
                    entity.ActualizadoPor = usuario;
                }
            }
        }
    }
}
