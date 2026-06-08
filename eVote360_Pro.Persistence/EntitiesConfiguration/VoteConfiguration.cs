using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360_Pro.Persistence.EntitiesConfiguration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Votes");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Election)
                .WithMany() 
                .HasForeignKey(x => x.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ElectedOffice)
                .WithMany() // Unidireccional
                .HasForeignKey(x => x.ElectedOfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Candidate)
                .WithMany() 
                .HasForeignKey(x => x.CandidateId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}