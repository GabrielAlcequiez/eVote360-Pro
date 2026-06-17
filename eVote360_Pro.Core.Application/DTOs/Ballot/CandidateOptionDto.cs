namespace eVote360_Pro.Core.Application.DTOs.Ballot
{
    public class CandidateOptionDto
    {
        public Guid CandidateId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAcronym { get; set; } = string.Empty;
        public string PartyLogo { get; set; } = string.Empty;
    }
}
