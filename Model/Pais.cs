using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Pais")]
    public partial class Pais
    {
        public Pais()
        {
            Alumnos = new List<Alumno>();
        }

        public int id { get; set; }
        public string Nombre { get; set; }
        
        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
