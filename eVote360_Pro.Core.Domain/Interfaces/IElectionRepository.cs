using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IElectionRepository : IBaseRepository<Election>
    {
        Task<bool> ValidateNoActiveElectionAsync();
        Task<Election?> GetActiveElectionAsync();
        Task<int> GetVoterCountByElectionAsync(Guid electionId);
    }
}