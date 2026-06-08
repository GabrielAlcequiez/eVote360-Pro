using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class PoliticalPartiesConfiguration : IEntityTypeConfiguration<PoliticalParty>
    {
        public void Configure(EntityTypeBuilder<PoliticalParty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("PoliticalParties");

            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(150); // tamaño no especificado en documento

            builder.Property(x=>x.Acronym)
                .IsRequired();
            builder.HasIndex(x=>x.Acronym)
                .IsUnique();

            builder.Property(x => x.Logo)
                .IsRequired();
            
            builder.Property(x=>x.Description)
                .HasMaxLength(300);

            builder.Property(x=> x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}