using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class ElectionConfiguration : IEntityTypeConfiguration<Election>
    {
        public void Configure(EntityTypeBuilder<Election> builder)
        {
            builder.ToTable("Elections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}