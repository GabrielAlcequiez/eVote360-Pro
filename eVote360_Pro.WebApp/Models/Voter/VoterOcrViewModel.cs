using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eVote360_Pro.WebApp.Models.Voter
{
    public class VoterOcrViewModel
    {
        [Required(ErrorMessage = "El número de documento es requerido.")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe subir una imagen de su cédula para validar su identidad.")]
        [Display(Name = "Foto de la Cédula")]
        public IFormFile DocumentImage { get; set; } = null!;
    }
}
