namespace eVote360_Pro.Core.Application.DTOs.Candidate
{
    public class CandidateUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
