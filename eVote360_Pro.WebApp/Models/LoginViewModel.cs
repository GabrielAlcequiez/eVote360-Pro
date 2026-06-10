using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es la clave de acceso del usuario.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
