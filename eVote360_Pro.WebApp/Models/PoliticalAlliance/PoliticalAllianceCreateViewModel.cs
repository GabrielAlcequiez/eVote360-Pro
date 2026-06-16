using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360_Pro.WebApp.Models.PoliticalAlliance
{
    public class PoliticalAllianceCreateViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un partido político.")]
        [Display(Name = "Partido Político")]
        public Guid ReceiverPartyId { get; set; }
        public List<SelectListItem> AvailableParties { get; set; } = [];
    }
}