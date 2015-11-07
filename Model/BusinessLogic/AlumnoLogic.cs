using Model.Repository;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Model.BusinessLogic
{
    public class AlumnoLogic : Repository<Alumno>
    {
        public List<Alumno> Listar()
        {
            using (this.ContextScope())
            {
                return this.GetAll().OrderBy(x => x.Apellido).ToList();
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

        public void ActualizarNombre(Alumno alumno)
        {
            using (this.ContextScope())
            {
                this.PartialUpdate(alumno, x => x.Nombre, x => x.Apellido);
                this.Save();
            }
        }
    }
}
