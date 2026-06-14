using eVote360_Pro.Core.Application.DTOs.PartyLeader;

namespace eVote360_Pro.WebApp.Models.PartyLeader
{
    public class PartyLeaderIndexViewModel
    {
        public List<PartyLeaderGetDto> Assignments { get; set; } = new();
        public bool HasActiveElection { get; set; }
    }
}
