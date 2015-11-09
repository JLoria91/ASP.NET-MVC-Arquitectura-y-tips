using Model.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Repository;

namespace Model.BusinessLogic
{
    public class AlumnoLogic : Repository<Alumno>
    {
        private ResponseModel rm;

        public AlumnoLogic()
        {
            rm = new ResponseModel();
        }

        public List<Alumno> Listar()
        {
            using (var db = this.ContextScope(new DemoContext()))
            {
                return this.GetAll(
                                x => x.Pais,
                                x => x.AlumnoCurso.Select(ac => ac.Alumno)
                            )
                           .OrderByDescending(x => x.id)
                           .ToList();
            }
        }

        public List<Alumno> ListarConQueryPersonalizado()
        {
            using (this.ContextScope(new DemoContext()))
            {
                return this.SqlQuery("SELECT * FROM Alumno")
                           .ToList();
            }
        }

        public ResponseModel Guardar(Alumno alumno)
        {
            using (this.ContextScope(new DemoContext()))
            {
                if (alumno.id == 0) this.Insert(alumno);
                else this.Update(alumno);

                this.Save();

                rm.SetResponse(true);
                return rm;
            }
        }

        public ResponseModel Eliminar(int id)
        {
            using (this.ContextScope(new DemoContext()))
            {
                this.PartialUpdate(new Alumno { id = id, Eliminado = true }, x => x.Eliminado);
                this.Save();

                rm.SetResponse(true);
                return rm;
            }
        }

        public Alumno Obtener(int id)
        {
            using (this.ContextScope(new DemoContext()))
            {
                return this.Get(
                    x => x.id == id,
                    x => x.AlumnoCurso.Select(ac => ac.Curso),
                    x => x.Pais
                );
            }
        }

        public void InsertarVarios(List<Alumno> alumnos)
        {
            using (this.ContextScope(new DemoContext()))
            {
                using (var trans = this.BeginsTransaction())
                {
                    try
                    {
                        this.Insert(alumnos);
                        this.Save();

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
            using (this.ContextScope(new DemoContext()))
            {
                this.PartialUpdate(alumno, x => x.Nombre, x => x.Apellido);
                this.Save();
            }
        }
    }
}
