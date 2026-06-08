using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
    {
        public void Configure(EntityTypeBuilder<Citizen> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Citizens");

            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(150); // tamaño no especificado en documento

            builder.Property(x => x.LastName)
                .IsRequired().HasMaxLength(150);

            builder.Property(x => x.Email)
                .IsRequired().HasMaxLength(150);
            builder.HasIndex(x=>x.Email)
                .IsUnique();

            builder.Property(x=>x.DocumentNumber)
                .IsRequired();
            builder.HasIndex(x=>x.DocumentNumber)
                .IsUnique();



        }
    }
}