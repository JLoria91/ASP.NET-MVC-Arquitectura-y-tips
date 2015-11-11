using System.ComponentModel.DataAnnotations;

namespace Demo_Arquitectura.ViewsModel
{
    public class LoginViewModel
    {
        [Required]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
}