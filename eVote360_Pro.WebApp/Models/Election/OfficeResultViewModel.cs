namespace eVote360_Pro.WebApp.Models.Election
{
    public class OfficeResultViewModel
    {
        public string OfficeName { get; set; } = string.Empty;
        public List<CandidateResultViewModel> Candidates { get; set; } = new();
        public int TotalVotes => Candidates.Sum(c => c.VoteCount);
    }

    public class CandidateResultViewModel
    {
        public string? CandidateName { get; set; }
        public string? Photo { get; set; }
        public string? PartyName { get; set; }
        public string? PartyAcronym { get; set; }
        public string? PartyLogo { get; set; }
        public int VoteCount { get; set; }
        public decimal Percentage => TotalVotes > 0 ? Math.Round((decimal)VoteCount / TotalVotes * 100, 1) : 0;

        public int TotalVotes { get; set; }
        public bool IsWinner { get; set; }
        public bool IsTie { get; set; }
    }
}
