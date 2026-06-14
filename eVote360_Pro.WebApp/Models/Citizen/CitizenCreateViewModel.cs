using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Citizen
{
    public class CitizenCreateViewModel
    {
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
        [MaxLength(20, ErrorMessage = "El número de documento no puede superar los 20 caracteres.")]
        public string DocumentNumber { get; set; } = string.Empty;
    }
}
