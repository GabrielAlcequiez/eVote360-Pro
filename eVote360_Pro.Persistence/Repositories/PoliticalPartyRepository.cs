using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using eVote360_Pro.Core.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class PoliticalPartyRepository(AppDbContext context) : BaseRepository<PoliticalParty>(context), IPoliticalPartyRepository
    {
        public async Task<IReadOnlyList<PoliticalParty>> GetAllWithPartyLeadersAsync()
        {
            return await _context.PoliticalParties
                .Include(p => p.PartyLeader)
                    .ThenInclude(pl => pl.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<PoliticalParty?> GetByAcronymAsync(string acronym)
        {
           return _context.PoliticalParties.FirstOrDefaultAsync(p => p.Acronym == acronym);
        }

        public async Task<bool> HasActiveCandidatesAsync(Guid id)
        {
            return await _context.Candidates.AnyAsync(c => c.PoliticalPartyId == id && c.IsActive);
        }

        public async Task<bool> HasActiveLeaderAsync(Guid id)
        {
            return await _context.PartyLeaders.AnyAsync(pl => pl.PoliticalPartyId == id && pl.User.IsActive);
        }

        public async Task<bool> HasParticipatedInElectionAsync(Guid id)
        {
            // 1. Verificar si hay votos registrados para candidatos de este partido
            var hasVotes = await _context.Votes.AnyAsync(v => v.Candidate != null && v.Candidate.PoliticalPartyId == id);
            if (hasVotes) return true;

            // 2. Verificar si hay asignaciones en elecciones activas o finalizadas
            var hasActiveOrFinalizedElection = await _context.Elections.AnyAsync(e => e.Status == ElectionStatus.Active || e.Status == ElectionStatus.Finalized);
            if (hasActiveOrFinalizedElection)
            {
                var hasAssignments = await _context.CandidateOfficeAssignments.AnyAsync(coa => coa.PoliticalPartyId == id);
                if (hasAssignments) return true;
            }

            return false;
        }
    }
}