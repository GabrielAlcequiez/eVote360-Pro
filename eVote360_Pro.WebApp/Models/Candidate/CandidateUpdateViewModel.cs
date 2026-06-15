using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Candidate
{
    public class CandidateUpdateViewModel
    {
        public Guid Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El nombre del candidato es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El apellido del candidato es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        public bool IsActive { get; set; }
    }
}
