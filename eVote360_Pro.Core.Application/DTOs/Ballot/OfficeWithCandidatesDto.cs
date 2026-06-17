namespace eVote360_Pro.Core.Application.DTOs.Ballot
{
    public class OfficeWithCandidatesDto
    {
        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public int PartyCount { get; set; }
        public int CandidateCount { get; set; }
        public List<CandidateOptionDto> Candidates { get; set; } = new();
    }
}
