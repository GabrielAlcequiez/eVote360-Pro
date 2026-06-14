using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IPartyLeaderRepository : IBaseRepository<PartyLeader>
    {
        Task DeletePhysicallyAsync(Guid userId);
        Task<List<PartyLeader>> GetAllWithDetailsAsync();
        Task<List<User>> GetAvailableLeadersAsync();
        Task<List<PoliticalParty>> GetAvailablePartiesAsync();

        Task<bool> HasPartyAssignmentAsync(Guid userId);

        Task<bool> HasActivePoliticalPartyAsync(Guid userId);
        
    }
}
