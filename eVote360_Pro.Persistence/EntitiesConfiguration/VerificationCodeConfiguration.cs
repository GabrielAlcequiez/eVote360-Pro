using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
    {
        public void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.ToTable("VerificationCodes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(6)
                .IsFixedLength()
                .IsRequired();

            builder.Property(x => x.GeneratedAt)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .IsRequired();

            builder.HasOne(x => x.Citizen)
                .WithMany() 
                .HasForeignKey(x => x.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Election)
                .WithMany() 
                .HasForeignKey(x => x.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}