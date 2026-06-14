using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class CandidateRepository(AppDbContext context) : BaseRepository<Candidate>(context), ICandidateRepository
    {
        public async Task<bool> HasBeenUsedInElectionAsync(Guid id)
        {
            return await _context.Votes.AnyAsync(x => x.CandidateId == id);
        }
    }
}