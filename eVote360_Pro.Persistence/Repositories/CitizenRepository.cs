using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class CitizenRepository(AppDbContext context) : BaseRepository<Citizen>(context), ICitizenRepository
    {
        public async Task<Citizen?> GetByDocumentNumber(string documentNumber)
        {
            return await _context.Citizens.FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        }

        public async Task<Citizen?> GetByEmail(string email)
        {
            return await _context.Citizens.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Citizen?> GetByName(string name)
        {
            return await _context.Citizens.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> HasBeenUsedInElectionAsync(Guid id, Guid electionId)
        {
            return await _context.CitizenParticipations.AnyAsync(x => x.CitizenId == id && x.ElectionId == electionId);
        }

        public async Task<bool> HasBeenUsedInAnyElectionAsync(Guid id)
        {
            return await _context.CitizenParticipations.AnyAsync(x => x.CitizenId == id);
        }

        public async Task AddParticipationAsync(CitizenParticipation participation)
        {
            await _context.CitizenParticipations.AddAsync(participation);
        }
    }
}