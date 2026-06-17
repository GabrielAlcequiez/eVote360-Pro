using eVote360_Pro.Core.Application.DTOs.Ballot;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IBallotService
    {
        Task<List<OfficeWithCandidatesDto>> GetOfficesWithCandidatesAsync(Guid electionId);
        Task FinalizeVotingAsync(Guid citizenId, Guid electionId, Dictionary<Guid, Guid?> selections, string citizenName, string citizenEmail, string electionName, DateTime electionDate);
    }
}
