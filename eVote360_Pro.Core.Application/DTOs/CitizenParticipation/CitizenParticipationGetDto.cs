namespace eVote360_Pro.Core.Application.DTOs.CitizenParticipation
{
    public class CitizenParticipationGetDto
    {
        public Guid Id { get; set; }
        public Guid CitizenId { get; set; }
        public string CitizenFullName { get; set; } = string.Empty;
        public string CitizenDocumentNumber { get; set; } = string.Empty;
        public Guid ElectionId { get; set; }
        public string ElectionName { get; set; } = string.Empty;
    }
}
