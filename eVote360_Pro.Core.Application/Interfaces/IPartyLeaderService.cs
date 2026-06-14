using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.PartyLeader;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.DTOs.User;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IPartyLeaderService
    {
        Task<List<PartyLeaderGetDto>> GetAllAsync();
        Task<List<UserGetDto>> GetAvailableLeadersAsync();
        Task<List<PoliticalPartyGetDto>> GetAvailablePartiesAsync();
        Task AssignAsync(PartyLeaderCreateDto dto);
        Task DeleteAsync(Guid userId);
    }
}