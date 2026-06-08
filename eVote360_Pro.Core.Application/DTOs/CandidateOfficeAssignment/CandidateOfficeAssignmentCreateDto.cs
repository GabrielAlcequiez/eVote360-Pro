namespace eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment
{
    public class CandidateOfficeAssignmentCreateDto
    {
        public Guid CandidateId { get; set; }
        public Guid ElectedOfficeId { get; set; }
        public Guid PoliticalPartyId { get; set; }
    }
}
