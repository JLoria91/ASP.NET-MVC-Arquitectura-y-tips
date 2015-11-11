namespace Model.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Usuario")]
    public partial class Usuario
    {
        public int id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
}
