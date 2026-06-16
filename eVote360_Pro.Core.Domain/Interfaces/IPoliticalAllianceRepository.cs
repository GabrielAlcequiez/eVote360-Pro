using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IPoliticalAllianceRepository : IBaseRepository<PoliticalAlliance>
    {
        Task<List<PoliticalAlliance>> GetAllByPartyIdWithDetailsAsync(Guid partyId);
        Task<bool> HasActiveAllianceBetweenPartiesAsync(Guid partyId1, Guid partyId2);
        Task<bool> HasPendingRequestBetweenPartiesAsync(Guid partyId1, Guid partyId2);
        Task<bool> HasActiveCandidatesAssignedBetweenPartiesAsync(Guid partyId1, Guid partyId2);
        Task DeletePhysicallyAsync(Guid id);
    }
}