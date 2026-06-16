using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360_Pro.WebApp.Models.CandidateOfficeAssignment
{
    public class CandidateOfficeAssignmentCreateViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un candidato.")]
        [Display(Name = "Candidato político")]
        public Guid CandidateId { get; set; }
        public List<SelectListItem> AvailableCandidates { get; set; } = [];

        [Required(ErrorMessage = "Debe seleccionar un puesto electivo.")]
        [Display(Name = "Puesto electivo")]
        public Guid ElectedOfficeId { get; set; }
        public List<SelectListItem> AvailableOffices { get; set; } = [];
    }
}
