namespace eVote360_Pro.Core.Application.DTOs.Election
{
    public class OfficeResultDto
    {
        public string OfficeName { get; set; } = string.Empty;
        public List<CandidateResultDto> Candidates { get; set; } = new();
        public int TotalVotes { get; set; }
    }

    public class CandidateResultDto
    {
        public string? CandidateName { get; set; }
        public string? Photo { get; set; }
        public string? PartyName { get; set; }
        public string? PartyAcronym { get; set; }
        public string? PartyLogo { get; set; }
        public int VoteCount { get; set; }
        public decimal Percentage { get; set; }
        public int TotalVotes { get; set; }
        public bool IsWinner { get; set; }
        public bool IsTie { get; set; }
    }
}
