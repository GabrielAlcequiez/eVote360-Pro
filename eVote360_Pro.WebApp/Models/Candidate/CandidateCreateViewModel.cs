using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Candidate
{
    public class CandidateCreateViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El nombre del candidato es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El apellido del candidato es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "La foto del candidato es requerida.")]
        public IFormFile? Photo { get; set; }
        // El partido político NO se selecciona en el formulario de creación;
        // se asigna automáticamente según el dirigente político autenticado.
    }
}
