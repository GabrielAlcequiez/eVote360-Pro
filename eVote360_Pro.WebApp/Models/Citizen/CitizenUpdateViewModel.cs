using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Citizen
{
    public class CitizenUpdateViewModel
    {
        [Required(ErrorMessage = "El ID del ciudadano es requerido.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [MaxLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es requerido.")]
        [RegularExpression(@"^\d{3}-?\d{7}-?\d{1}$", ErrorMessage = "El formato de la cédula dominicana debe ser ###-#######-# o 11 dígitos.")]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "El número de documento debe tener exactamente 11 dígitos.")]
        public string DocumentNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
