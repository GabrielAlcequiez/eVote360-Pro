using eVote360_Pro.Core.Application.DTOs.ElectedOffice;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IElectedOfficeService
    {
        Task<ElectedOfficeGetDto> AddAsync(ElectedOfficeCreateDto dto);
        Task UpdateAsync(ElectedOfficeUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<ElectedOfficeGetDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<ElectedOfficeGetDto>> GetAllAsync();        
    }
}