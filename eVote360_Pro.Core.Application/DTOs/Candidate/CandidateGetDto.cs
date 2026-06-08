namespace eVote360_Pro.Core.Application.DTOs.Candidate
{
    public class CandidateGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{Name} {LastName}";
        public string Photo { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Información del partido político al que pertenece
        public Guid PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = string.Empty;
        public string PoliticalPartyAcronym { get; set; } = string.Empty;
        public string PoliticalPartyLogo { get; set; } = string.Empty;
    }
}
