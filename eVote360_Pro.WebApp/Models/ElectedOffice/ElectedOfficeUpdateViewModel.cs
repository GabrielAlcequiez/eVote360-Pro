using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.ElectedOffice
{
    public class ElectedOfficeUpdateViewModel
    {
        [Required(ErrorMessage = "El ID del cargo electivo es requerido.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre del puesto electivo es requerido.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del cargo electivo es requerida.")]
        [MaxLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
