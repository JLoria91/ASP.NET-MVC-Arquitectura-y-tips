using Model.Repository;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Model.BusinessLogic
{
    public class AlumnoLogic : Repository<Alumno>
    {
        public List<Alumno> Listar()
        {
            using (var db = this.ContextScope())
            {
                return this.GetAll(
                                x => x.AlumnoCurso
                                      .Select(ac => ac.Curso)
                            )
                           .OrderBy(x => x.Apellido)
                           .ToList();
            }
        }

        public List<Alumno> ListarConQueryPersonalizado()
        {
            using (this.ContextScope())
            {
                return this.SqlQuery("SELECT * FROM Alumno")
                           .ToList();
            }
        }

        public Alumno Buscar(int id)
        {
            using (this.ContextScope())
            {
                return this.FindBy(x => x.id == id)
                           .FirstOrDefault();
            }
        }

        public void Guardar(Alumno alumno)
        {
            using (this.ContextScope())
            {
                if (alumno.id == 0) this.Insert(alumno);
                else this.Update(alumno);

                this.Save();
            }
        }

        public Alumno Get(int id)
        {
            using (this.ContextScope())
            {
                return this.Get(x => x.id == id);
            }
        }

        public void InsertarVarios(List<Alumno> alumnos)
        {
            using (this.ContextScope())
            {
                this.Insert(alumnos);
                this.Save();
            }
        }

        public void ActualizarNombre(Alumno alumno)
        {
            using (this.ContextScope())
            {
                this.PartialUpdateOrInsert(alumno, x => x.Nombre, x => x.Apellido);
                this.Save();
            }
        }
    }
}
