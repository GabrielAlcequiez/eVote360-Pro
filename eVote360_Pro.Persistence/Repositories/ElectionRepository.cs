using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class ElectionRepository(AppDbContext context) : BaseRepository<Election>(context), IElectionRepository
    {
        public async Task<bool> ValidateNoActiveElectionAsync()
        {
            return await _context.Elections.AnyAsync(e => e.Status == ElectionStatus.Active);
        }

        public async Task<Election?> GetActiveElectionAsync()
        {
            return await _context.Elections.FirstOrDefaultAsync(e => e.Status == ElectionStatus.Active);
        }
    }
}