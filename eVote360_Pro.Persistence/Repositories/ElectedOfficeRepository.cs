using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Repositories
{
    public class ElectedOfficeRepository(AppDbContext context) : BaseRepository<ElectedOffice>(context), IElectedOfficeRepository
    {
        public async Task<ElectedOffice?> GetByName(string name)
        {
            return await _context.ElectedOffices.FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}
