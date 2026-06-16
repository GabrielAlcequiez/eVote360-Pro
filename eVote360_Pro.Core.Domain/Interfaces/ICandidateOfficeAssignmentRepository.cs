using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface ICandidateOfficeAssignmentRepository : IBaseRepository<CandidateOfficeAssignment>
    {
        Task<bool> CandidateHasOfficeAssigned(Guid candidateId, Guid partyId);
        Task<bool> OfficeHasCandidateAssigned(Guid officeId, Guid partyId);
        Task<CandidateOfficeAssignment?> GetAssignmentByCandidateAndParty(Guid candidateId, Guid partyId);
        Task DeletePhysicallyAsync(Guid id);
    }
}