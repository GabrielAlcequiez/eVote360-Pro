using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class CandidateOfficeAssignmentRepository(AppDbContext context) : BaseRepository<CandidateOfficeAssignment>(context), ICandidateOfficeAssignmentRepository
    {
        public async Task<bool> CandidateHasOfficeAssigned(Guid candidateId, Guid partyId)
        {
            return await _context.CandidateOfficeAssignments
                .AnyAsync(x => x.CandidateId == candidateId && x.PoliticalPartyId == partyId);
        }

        public async Task<bool> OfficeHasCandidateAssigned(Guid officeId, Guid partyId)
        {
            return await _context.CandidateOfficeAssignments
                .AnyAsync(x => x.ElectedOfficeId == officeId && x.PoliticalPartyId == partyId);
        }

        public async Task<CandidateOfficeAssignment?> GetAssignmentByCandidateAndParty(Guid candidateId, Guid partyId)
        {
            return await _context.CandidateOfficeAssignments
                .FirstOrDefaultAsync(x => x.CandidateId == candidateId && x.PoliticalPartyId == partyId);
        }

        public async Task DeletePhysicallyAsync(Guid id)
        {
            var entity = await _context.CandidateOfficeAssignments
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                return;

            _context.CandidateOfficeAssignments.Remove(entity);
        }

        public async Task<CandidateOfficeAssignment?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.CandidateOfficeAssignments
                .Include(x => x.Candidate)
                .Include(x => x.ElectedOffice)
                .Include(x => x.PoliticalParty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<CandidateOfficeAssignment>> GetAllWithDetailsAsync()
        {
            return await _context.CandidateOfficeAssignments
                .Include(x => x.Candidate)
                .Include(x => x.ElectedOffice)
                .Include(x => x.PoliticalParty)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CandidateOfficeAssignment>> GetAllByPartyIdWithDetailsAsync(Guid partyId)
        {
            return await _context.CandidateOfficeAssignments
                .Include(x => x.Candidate)
                .Include(x => x.ElectedOffice)
                .Include(x => x.PoliticalParty)
                .Where(x => x.PoliticalPartyId == partyId)
                .ToListAsync();
        }
    }
}