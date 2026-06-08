using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class PartyLeaderConfiguration : IEntityTypeConfiguration<PartyLeader>
    {
        public void Configure(EntityTypeBuilder<PartyLeader> builder)
        {
            builder.ToTable("PartyLeaders");

            // composite primary key
            builder.HasKey(x => new { x.PartyLeaderId, x.PoliticalPartyId });

            builder.HasIndex(x => x.PartyLeaderId)
                .IsUnique();

            builder.HasIndex(x => x.PoliticalPartyId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne() 
                .HasForeignKey<PartyLeader>(x => x.PartyLeaderId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.PoliticalParty)
                .WithOne() 
                .HasForeignKey<PartyLeader>(x => x.PoliticalPartyId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}