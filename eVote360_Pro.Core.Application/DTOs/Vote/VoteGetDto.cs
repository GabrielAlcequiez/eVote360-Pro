namespace eVote360_Pro.Core.Application.DTOs.Vote
{
    public class VoteGetDto
    {
        public Guid Id { get; set; }

        // Elección
        public Guid ElectionId { get; set; }
        public string ElectionName { get; set; } = string.Empty;

        // Cargo electivo
        public Guid ElectedOfficeId { get; set; }
        public string ElectedOfficeName { get; set; } = string.Empty;

        // Candidato (null si fue voto en blanco)
        public Guid? CandidateId { get; set; }
        public string? CandidateFullName { get; set; }
    }
}
