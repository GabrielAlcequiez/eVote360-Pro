using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class CitizenParticipationConfiguration : IEntityTypeConfiguration<CitizenParticipation>
    {
        public void Configure(EntityTypeBuilder<CitizenParticipation> builder)
        {
            builder.ToTable("CitizenParticipations");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Election)
                .WithMany() 
                .HasForeignKey(x => x.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Citizen)
                .WithMany() 
                .HasForeignKey(x => x.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice compuesto único para garantizar que un ciudadano vote máximo una vez por elección
            builder.HasIndex(x => new { x.ElectionId, x.CitizenId })
                .IsUnique();
        }
    }
}