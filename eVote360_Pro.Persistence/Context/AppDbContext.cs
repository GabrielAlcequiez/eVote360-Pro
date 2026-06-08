using System.Reflection;
using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.Persistence.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ElectedOffice> ElectedOffices => Set<ElectedOffice>();
        public DbSet<Citizen> Citizens => Set<Citizen>();
        public DbSet<PoliticalParty> PoliticalParties => Set<PoliticalParty>();
        public DbSet<User> Users => Set<User>();
        public DbSet<PartyLeader> PartyLeaders => Set<PartyLeader>();
        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<PoliticalAlliance> PoliticalAlliances => Set<PoliticalAlliance>();
        public DbSet<CandidateOfficeAssignment> CandidateOfficeAssignments => Set<CandidateOfficeAssignment>();
        public DbSet<Election> Elections => Set<Election>();
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
        public DbSet<CitizenParticipation> CitizenParticipations => Set<CitizenParticipation>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}