using eVote360_Pro.Core.Application.DTOs.PoliticalParty;

namespace eVote360_Pro.WebApp.Models.PoliticalParty
{
    public class PoliticalPartyIndexViewModel
    {
        public List<PoliticalPartyGetDto> Parties { get; set; } = new();
        public bool HasActiveElection { get; set; }
    }
}
