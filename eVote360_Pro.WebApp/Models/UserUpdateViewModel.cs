using System.ComponentModel.DataAnnotations;
using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.WebApp.Models
{
    public class UserUpdateViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El formato de correo electrónico no es válido.")]
        [MaxLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres.")]
        public string Username { get; set; } = string.Empty;

        // Opcional en edición
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe contener al menos una letra y un número.")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El rol es requerido.")]
        public Role Role { get; set; }

        public bool IsActive { get; set; }
    }
}
