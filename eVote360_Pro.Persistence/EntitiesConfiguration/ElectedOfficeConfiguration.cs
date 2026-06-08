using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class ElectedOfficeConfiguration : IEntityTypeConfiguration<ElectedOffice>
    {
        public void Configure(EntityTypeBuilder<ElectedOffice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("ElectedOffices");

            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(150); // tamaño no especificado en documento
            builder.HasIndex(x=>x.Name)
                .IsUnique();
                
            builder.Property(x => x.Description)
                .IsRequired().HasMaxLength(300);

            builder.Property(x=> x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}