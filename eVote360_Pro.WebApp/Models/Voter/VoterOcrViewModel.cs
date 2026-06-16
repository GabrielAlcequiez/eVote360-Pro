using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eVote360_Pro.WebApp.Models.Voter
{
    public class VoterOcrViewModel
    {
        [Required(ErrorMessage = "El número de documento es requerido.")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe cargar la foto de su cédula.")]
        [Display(Name = "Foto de la Cédula")]
        public IFormFile DocumentImage { get; set; } = null!;
    }
}
