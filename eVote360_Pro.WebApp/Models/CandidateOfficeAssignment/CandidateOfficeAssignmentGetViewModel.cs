namespace eVote360_Pro.WebApp.Models.CandidateOfficeAssignment
{
    public class CandidateOfficeAssignmentGetViewModel
    {
        public Guid Id { get; set; }
        public string CandidateFullName { get; set; } = string.Empty;
        public string CandidatePhoto { get; set; } = string.Empty;
        public string ElectedOfficeName { get; set; } = string.Empty;
        public string PoliticalPartyName { get; set; } = string.Empty;
        public string PoliticalPartyAcronym { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
