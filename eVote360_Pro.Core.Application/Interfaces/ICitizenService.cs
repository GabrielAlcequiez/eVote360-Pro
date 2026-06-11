using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.Citizen;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface ICitizenService
    {
        Task<CitizenGetDto> AddAsync(CitizenCreateDto dto);
        Task UpdateAsync(CitizenUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<CitizenGetDto?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<CitizenGetDto>> GetAllAsync(); 

    }
}