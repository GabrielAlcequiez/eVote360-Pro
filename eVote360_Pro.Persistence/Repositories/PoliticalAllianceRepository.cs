using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class PoliticalAllianceRepository(AppDbContext context) : BaseRepository<PoliticalAlliance>(context), IPoliticalAllianceRepository
    {
        public async Task DeletePhysicallyAsync(Guid id)
        {
            var entity = await _context.PoliticalAlliances.FindAsync(id);
            if (entity != null) _context.PoliticalAlliances.Remove(entity);
        }

        public async Task<List<PoliticalAlliance>> GetAllByPartyIdWithDetailsAsync(Guid partyId)
        {
            return await _context.PoliticalAlliances
            .Include(pa => pa.RequesterParty)
            .Include(pa => pa.ReceiverParty)
            .Where(pa => pa.RequesterPartyId == partyId || pa.ReceiverPartyId == partyId).ToListAsync();
        }

        public async Task<bool> HasActiveAllianceBetweenPartiesAsync(Guid partyId1, Guid partyId2)
        {
            return await _context.PoliticalAlliances
                  .AnyAsync(pa => pa.Status == AllianceStatus.Accepted &&
                      ((pa.RequesterPartyId == partyId1 && pa.ReceiverPartyId == partyId2) ||
                       (pa.RequesterPartyId == partyId2 && pa.ReceiverPartyId == partyId1)));
        }

        public async Task<bool> HasActiveCandidatesAssignedBetweenPartiesAsync(Guid partyId1, Guid partyId2)
        {
            return await _context.CandidateOfficeAssignments
               .Include(coa => coa.Candidate)
               .AnyAsync(coa => coa.Type == CandidacyType.Alliance &&
                   ((coa.PoliticalPartyId == partyId1 && coa.Candidate.PoliticalPartyId == partyId2) ||
                    (coa.PoliticalPartyId == partyId2 && coa.Candidate.PoliticalPartyId == partyId1)));
        }

        public async Task<bool> HasPendingRequestBetweenPartiesAsync(Guid partyId1, Guid partyId2)
        {
            return await _context.PoliticalAlliances
            .AnyAsync(pa => pa.Status == AllianceStatus.Pending &&
                    ((pa.RequesterPartyId == partyId1 && pa.ReceiverPartyId == partyId2) ||
                     (pa.RequesterPartyId == partyId2 && pa.ReceiverPartyId == partyId1)));
        }
    }
}