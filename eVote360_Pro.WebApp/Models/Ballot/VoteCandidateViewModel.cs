namespace eVote360_Pro.WebApp.Models.Ballot
{
    public class VoteCandidateViewModel
    {
        public Guid OfficeId { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public List<CandidateOptionViewModel> Candidates { get; set; } = new();
        public Guid? SelectedCandidateId { get; set; }
    }

    public class CandidateOptionViewModel
    {
        public Guid CandidateId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string PartyAcronym { get; set; } = string.Empty;
        public string PartyLogo { get; set; } = string.Empty;
    }
}
