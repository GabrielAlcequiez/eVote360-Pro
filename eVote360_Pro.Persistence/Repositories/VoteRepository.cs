using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class VoteRepository(AppDbContext context) : BaseRepository<Vote>(context), IVoteRepository
    {
        public async Task<List<Vote>> GetVotesByElectionWithDetailsAsync(Guid electionId)
        {
            return await _context.Votes
                .Include(v => v.ElectedOffice)
                .Include(v => v.Candidate)
                    .ThenInclude(c => c!.PoliticalParty)
                .Where(v => v.ElectionId == electionId)
                .ToListAsync();
        }
    }
}
