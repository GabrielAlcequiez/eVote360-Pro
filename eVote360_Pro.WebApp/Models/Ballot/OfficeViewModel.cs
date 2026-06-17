namespace eVote360_Pro.WebApp.Models.Ballot
{
    public class OfficeViewModel
    {
        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public int PartyCount { get; set; }
        public int CandidateCount { get; set; }
        public bool HasSelection { get; set; }
    }
}
