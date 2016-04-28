namespace Model
{
    using Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    [Table("Alumno")]
    public partial class Alumno : IAuditable
    {
        public Alumno()
        {
            Adjunto = new List<Adjunto>();
            AlumnoCurso = new List<AlumnoCurso>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        public int Sexo { get; set; }

        public bool Eliminado { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Nacimiento")]
        public string FechaNacimiento { get; set; }

        public int? Pais_id {get;set;}
        public virtual Pais Pais {get; set;}

        public virtual ICollection<Adjunto> Adjunto { get; set; }

        public virtual ICollection<AlumnoCurso> AlumnoCurso { get; set; }

        #region Auditoria
        public int CreadoPor { get; set; }
        public DateTime CreadoFecha { get; set; }
        public int ActualizadoPor { get; set; }
        public DateTime ActualizadoFecha { get; set; }
        #endregion
    }
}
