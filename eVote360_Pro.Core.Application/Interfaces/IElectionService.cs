using eVote360_Pro.Core.Application.DTOs.Election;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IElectionService
    {
        Task<ElectionGetDto?> AddElection(ElectionCreateDto dto);
        Task<bool> ActivateElection(Guid id);
        Task<bool> FinishElection(Guid id);
        Task<IReadOnlyList<ElectionListDto>> GetAllAsync();
        Task<ElectionGetDto?> GetByIdAsync(Guid id);
        Task<List<OfficeResultDto>> GetElectionResultsAsync(Guid electionId);
    }
}