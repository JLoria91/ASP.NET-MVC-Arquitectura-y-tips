using Model.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Repository;

namespace Model.BusinessLogic
{
    public class AlumnoLogic
    {
        private ResponseModel rm;
        private Repository<Alumno> repo;

        public AlumnoLogic()
        {
            rm = new ResponseModel();
            repo = new Repository<Alumno>();
        }

        public List<Alumno> Listar()
        {
            using (var db = repo.ContextScope(new DemoContext()))
            {
                return repo.GetAll(
                                x => x.Pais,
                                x => x.AlumnoCurso.Select(ac => ac.Alumno)
                            )
                           .OrderByDescending(x => x.id)
                           .ToList();
            }
        }

        public List<Alumno> ListarConQueryPersonalizado()
        {
            using (repo.ContextScope(new DemoContext()))
            {
                return repo.SqlQuery("SELECT * FROM Alumno")
                           .ToList();
            }
        }

        public ResponseModel Guardar(Alumno alumno)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                if (alumno.id == 0) repo.Insert(alumno);
                else repo.Update(alumno);

                repo.Save();

                rm.SetResponse(true);
                return rm;
            }
        }

        public ResponseModel Eliminar(int id)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                repo.PartialUpdate(new Alumno { id = id, Eliminado = true }, x => x.Eliminado);
                repo.Save();

                rm.SetResponse(true);
                return rm;
            }
        }

        public Alumno Obtener(int id)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                return repo.Get(
                    x => x.id == id,
                    x => x.AlumnoCurso.Select(ac => ac.Curso),
                    x => x.Pais
                );
            }
        }

        public void InsertarVarios(List<Alumno> alumnos)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                using (var trans = repo.BeginsTransaction())
                {
                    try
                    {
                        repo.Insert(alumnos);
                        repo.Save();

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }
        }

        public void ActualizarNombre(Alumno alumno)
        {
            using (repo.ContextScope(new DemoContext()))
            {
                repo.PartialUpdate(alumno, x => x.Nombre, x => x.Apellido);
                repo.Save();
            }
        }
    }
}
