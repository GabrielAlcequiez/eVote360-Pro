namespace eVote360_Pro.Core.Application.DTOs.PartyLeader
{
    public class PartyLeaderGetDto
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserUsername { get; set; } = string.Empty;

        public Guid PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = string.Empty;
        public string PoliticalPartyAcronym { get; set; } = string.Empty;
    }
}
