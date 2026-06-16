using eVote360_Pro.Core.Application.DTOs.Candidate;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface ICandidateService
    {
        Task<CandidateGetDto> AddAsync(CandidateCreateDto dto, Guid userId);
        Task UpdateAsync(CandidateUpdateDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
        Task<CandidateGetDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<CandidateGetDto>> GetAllAsync(Guid userId);
    }
}