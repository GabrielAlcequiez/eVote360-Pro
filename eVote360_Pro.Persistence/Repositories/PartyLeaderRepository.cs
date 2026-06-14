
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class PartyLeaderRepository(AppDbContext context) : BaseRepository<PartyLeader>(context), IPartyLeaderRepository
    {
        public async Task DeletePhysicallyAsync(Guid userId)
        {
            var leader = await _context.PartyLeaders
                           .FirstOrDefaultAsync(pl => pl.UserId == userId);
            if (leader == null) return;  // si no existe, no hay nada que eliminar
            _context.PartyLeaders.Remove(leader);
        }

        public async Task<List<PartyLeader>> GetAllWithDetailsAsync()
        {
            return await _context.PartyLeaders.Include(p => p.User).Include(p => p.PoliticalParty).ToListAsync();
        }

        public async Task<List<User>> GetAvailableLeadersAsync()
        {
            var assignedUserIds = await _context.PartyLeaders.Select(p => p.UserId).ToListAsync();
            return await _context.Users.Where(u => u.IsActive == true && u.Role == Role.PoliticalLeader && !assignedUserIds.Contains(u.Id)).ToListAsync();
        }

        public async Task<List<PoliticalParty>> GetAvailablePartiesAsync()
        {
            var assignedPartyIds = await _context.PartyLeaders
                .Select(pl => pl.PoliticalPartyId)
                .ToListAsync();
            return await _context.PoliticalParties.Where(u => u.IsActive == true && !assignedPartyIds.Contains(u.Id)).ToListAsync();
        }
    }
}