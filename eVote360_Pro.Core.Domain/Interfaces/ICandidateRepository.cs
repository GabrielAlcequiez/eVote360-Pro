using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface ICandidateRepository : IBaseRepository<Candidate>
    {
        Task<bool> HasBeenUsedInElectionAsync(Guid id);
        Task<IReadOnlyList<Candidate>> GetAllWithParty(Guid partyId);
    }
}