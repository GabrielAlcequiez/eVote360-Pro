using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidates");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Photo)
                .IsRequired();

            builder.Property(x => x.PoliticalPartyId)
            .IsRequired();

            builder.HasOne(x => x.PoliticalParty)
                   .WithMany(x => x.Candidates)
                   .HasForeignKey(x => x.PoliticalPartyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ElectedOffice)
                   .WithMany(x => x.Candidates)
                   .HasForeignKey(x => x.ElectedOfficeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}