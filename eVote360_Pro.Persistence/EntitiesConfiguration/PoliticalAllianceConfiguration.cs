using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class PoliticalAllianceConfiguration : IEntityTypeConfiguration<PoliticalAlliance>
    {
        public void Configure(EntityTypeBuilder<PoliticalAlliance> builder)
        {
            builder.ToTable("PoliticalAlliances");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequestDate)
                .IsRequired();

            // mapeados a int, más rápido para este caso
            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();


            builder.HasOne(x => x.RequesterParty)
                .WithMany()
                .HasForeignKey(x => x.RequesterPartyId)
                .OnDelete(DeleteBehavior.Restrict); 
            builder.HasOne(x => x.ReceiverParty)
                .WithMany() 
                .HasForeignKey(x => x.ReceiverPartyId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}