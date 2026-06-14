using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace eVote360_Pro.WebApp.Models.PoliticalParty
{
    public class PoliticalPartyCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del partido es requerido.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        [Display(Name = "Nombre del Partido")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El acrónimo es requerido.")]
        [StringLength(10, ErrorMessage = "El acrónimo no puede superar los 10 caracteres.")]
        [Display(Name = "Siglas / Acrónimo")]
        public string Acronym { get; set; } = string.Empty;

        [Required(ErrorMessage = "El logo del partido es requerido.")]
        [Display(Name = "Logo del Partido")]
        public IFormFile LogoFile { get; set; } = null!;
    }
}
