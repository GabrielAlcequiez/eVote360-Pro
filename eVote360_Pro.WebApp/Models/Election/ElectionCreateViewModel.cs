using System.ComponentModel.DataAnnotations;

namespace eVote360_Pro.WebApp.Models.Election
{
    public class ElectionCreateViewModel
    {
        [Required(ErrorMessage = "El nombre de la elección es requerido.")]
        [MaxLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de realización es requerida.")]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; } = DateTime.Today;
    }
}
