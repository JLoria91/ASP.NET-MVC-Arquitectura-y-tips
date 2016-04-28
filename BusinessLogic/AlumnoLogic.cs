using System.Collections.Generic;
using System;
using System.Linq;
using Common;
using Repository.Interfaces;
using Model;
using Repository;

namespace BusinessLogic
{
    public interface IAlumnoLogic {
        List<Alumno> Listar();
        List<Alumno> ListarConQueryPersonalizado();
        ResponseModel Guardar(Alumno alumno);
        ResponseModel Eliminar(int id);
        Alumno Obtener(int id);
        void InsertarVarios(List<Alumno> alumnos);
        void ActualizarNombre(Alumno alumno);
    }

    public class AlumnoLogic : IAlumnoLogic
    {
        private ResponseModel rm;
        private readonly IRepository<Alumno> repoAlumno;

        public AlumnoLogic(IRepository<Alumno> _repoAlumno)
        {
            rm = new ResponseModel();
            repoAlumno = _repoAlumno;
        }

        public List<Alumno> Listar()
        {
            using (var ctx = new DbContextScope())
            {
                return repoAlumno.GetAll(
                                x => x.Pais,
                                x => x.AlumnoCurso.Select(ac => ac.Curso),
                                x => x.Adjunto
                            )
                           .Where(x => !x.Eliminado)
                           .OrderByDescending(x => x.id)
                           .ToList();
            }
        }

        public List<Alumno> ListarConQueryPersonalizado()
        {
            using (var ctx = new DbContextScope())
            {
                return repoAlumno.SqlQuery("SELECT * FROM Alumno")
                           .ToList();
            }
        }

        public ResponseModel Guardar(Alumno alumno)
        {
            using (var ctx = new DbContextScope())
            {
                if (alumno.id == 0) repoAlumno.Insert(alumno);
                else repoAlumno.Update(alumno);

                ctx.SaveChanges();

                rm.SetResponse(true);
                return rm;
            }
        }

        public ResponseModel Eliminar(int id)
        {
            using (var ctx = new DbContextScope())
            {
                repoAlumno.PartialUpdate(new Alumno { id = id, Eliminado = true }, x => x.Eliminado);
                ctx.SaveChanges();

                rm.SetResponse(true);
                return rm;
            }
        }

        public Alumno Obtener(int id)
        {
            using (var ctx = new DbContextScope())
            {
                return repoAlumno.Get(
                    x => x.id == id,
                    x => x.AlumnoCurso.Select(ac => ac.Curso),
                    x => x.Pais
                );
            }
        }

        public void InsertarVarios(List<Alumno> alumnos)
        {
            using (var ctx = new DbContextScope())
            {
                using (DbContextScope.DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        repoAlumno.Insert(alumnos);
                        ctx.SaveChanges();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        public void ActualizarNombre(Alumno alumno)
        {
            using (var ctx = new DbContextScope())
            {
                repoAlumno.PartialUpdate(alumno, x => x.Nombre, x => x.Apellido);
                ctx.SaveChanges();
            }
        }
    }
}
