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

            builder.HasKey(x => x.UserId);

            builder.HasIndex(x => x.PoliticalPartyId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.PartyLeader)
                .HasForeignKey<PartyLeader>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PoliticalParty)
                .WithOne(x => x.PartyLeader)
                .HasForeignKey<PartyLeader>(x => x.PoliticalPartyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}