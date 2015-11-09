namespace Model
{
    using System.Data.Entity;
    using Entities;
    using System;
    using Entities.Interfaces;
    using System.Data.Entity.Validation;

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
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        
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

            IgnorarEliminados();
        }

        public override int SaveChanges()
        {
            Auditar();
            return base.SaveChanges();
        }

        void IgnorarEliminados()
        {
            // Falta implementar, este código lo que va hacer es eliminar aquellas consultas que tengan un campo Eliminado seteado a True
        }

        /// <summary>
        /// Registra los cambios realizados a las clases que implementen la interface IAuditoria
        /// </summary>
        void Auditar()
        {
            foreach (var e in ChangeTracker.Entries<IAuditoria>())
            {
                var _audit = new Auditoria();

                _audit.Tabla = e.Entity.GetType().Name;
                _audit.Tabla_id = e.Property(x => x.id).CurrentValue;

                if (e.State == EntityState.Added)
                    _audit.Tipo = AuditoriaTipo.Insertar;

                if (e.State == EntityState.Deleted)
                    _audit.Tipo = AuditoriaTipo.Eliminar;

                if (e.State == EntityState.Modified)
                    _audit.Tipo = AuditoriaTipo.Actualizar;

                _audit.Fecha = DateTime.Now;

                this.Entry(_audit).State = EntityState.Added;
            }
        }
    }
}
