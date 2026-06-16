using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IPoliticalPartyRepository : IBaseRepository<PoliticalParty>
    {
        Task<PoliticalParty?> GetByAcronymAsync(string acronym);
        Task<bool> HasActiveCandidatesAsync(Guid id);
        Task<bool> HasActiveLeaderAsync(Guid id);
        Task<bool> HasParticipatedInElectionAsync(Guid id);
        Task<IReadOnlyList<PoliticalParty>> GetAllWithPartyLeadersAsync();
    }
}