using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360_Pro.WebApp.Models.PartyLeader
{
    public class PartyLeaderCreateViewModel
    {
        [Required(ErrorMessage = "El dirigente político es requerido.")]
        [Display(Name = "Dirigente Político")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "El partido político es requerido.")]
        [Display(Name = "Partido Político")]
        public Guid PoliticalPartyId { get; set; }

        public List<SelectListItem> AvailableLeaders { get; set; } = new();
        public List<SelectListItem> AvailableParties { get; set; } = new();
    }
}
