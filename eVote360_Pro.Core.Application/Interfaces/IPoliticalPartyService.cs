using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IPoliticalPartyService
    {
        Task<PoliticalPartyGetDto> AddAsync(PoliticalPartyCreateDto dto);
        Task UpdateAsync(PoliticalPartyUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<PoliticalPartyGetDto?> GetByIdAsync(Guid id);
        Task<List<PoliticalPartyGetDto>> GetAllAsync();
        Task<bool> HasParticipatedInElectionAsync(Guid id);
        Task<bool> HasActiveCandidatesAsync(Guid id);
    }
}