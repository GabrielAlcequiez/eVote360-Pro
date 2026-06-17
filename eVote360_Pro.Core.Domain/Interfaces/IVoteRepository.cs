using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IVoteRepository : IBaseRepository<Vote>
    {
        Task<List<Vote>> GetVotesByElectionWithDetailsAsync(Guid electionId);
    }
}
