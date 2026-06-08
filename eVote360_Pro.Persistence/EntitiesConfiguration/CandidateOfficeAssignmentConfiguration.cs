using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class CandidateOfficeAssignmentConfiguration : IEntityTypeConfiguration<CandidateOfficeAssignment>
    {
        public void Configure(EntityTypeBuilder<CandidateOfficeAssignment> builder)
        {
            builder.ToTable("CandidateOfficeAssignments");
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.CreatedAt)
                .IsRequired();

            builder.Property(x=>x.Type)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(x => x.ElectedOffice)
                .WithMany() // Unidirectional
                .HasForeignKey(x => x.ElectedOfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Candidate)
                .WithMany() // Unidirectional
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PoliticalParty)
                .WithMany() // Unidirectional
                .HasForeignKey(x => x.PoliticalPartyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}