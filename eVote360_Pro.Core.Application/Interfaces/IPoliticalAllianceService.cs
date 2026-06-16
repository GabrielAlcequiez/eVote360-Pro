using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IPoliticalAllianceService
    {
        Task<List<PoliticalAllianceGetDto>> GetPendingRequestAsync(Guid partyId);
        Task<List<PoliticalAllianceGetDto>> GetSentRequestAsync(Guid partyId);
        Task<List<PoliticalAllianceGetDto>> GetActiveAllianceAsync(Guid partyId);
        Task<List<PoliticalPartyGetDto>> GetAvailablePartiesForAllAllianceAsync(Guid currentPartyId);
        Task CreateRequestAsync(Guid requesterPartyId, PoliticalAllianceCreateDto dto);

        Task AcceptRequestAsync(Guid id, Guid currentPartyId);
        Task RejectRequestAsync(Guid id, Guid currentPartyId);
        Task DeleteRequestAsync(Guid id, Guid currentPartyId);
        Task DeleteAllianceAsync(Guid id, Guid currentPartyId);
        Task<PoliticalAllianceGetDto?> GetByIdAsync(Guid id);

    }
}