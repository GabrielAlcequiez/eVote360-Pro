using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Voter
{
    public class VoterDocumentViewModel
    {
        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [RegularExpression(@"^\d{3}-?\d{7}-?\d{1}$", ErrorMessage = "El formato de la cédula dominicana debe ser ###-#######-# o 11 dígitos.")]
        [Display(Name = "Número de Cédula")]
        public string DocumentNumber { get; set; } = string.Empty;
    }
}
