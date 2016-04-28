namespace Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Curso")]
    public partial class Curso
    {
        public Curso()
        {
            AlumnoCurso = new List<AlumnoCurso>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<AlumnoCurso> AlumnoCurso { get; set; }
    }
}
