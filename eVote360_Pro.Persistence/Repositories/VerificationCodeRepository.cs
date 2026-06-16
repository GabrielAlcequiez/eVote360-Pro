using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class VerificationCodeRepository(AppDbContext context) : BaseRepository<VerificationCode>(context), IVerificationCodeRepository
    {
        public async Task<VerificationCode?> GetLatestUnusedCodeAsync(Guid citizenId, Guid electionId)
        {
            return await _context.VerificationCodes
                .Where(x => x.CitizenId == citizenId && x.ElectionId == electionId && !x.IsUsed)
                .OrderByDescending(x => x.GeneratedAt)
                .FirstOrDefaultAsync();
        }
    }
}
