using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Users");

            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(150); // tamaño no especificado en documento

            builder.Property(x => x.LastName)
                .IsRequired().HasMaxLength(150);

            builder.Property(x => x.Email)
                .IsRequired().HasMaxLength(150);
            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Username)
                .IsRequired();
            builder.HasIndex(x => x.Username)
               .IsUnique();

            builder.Property(x => x.Password)
                .IsRequired();
            
            builder.Property(x=>x.Role)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}