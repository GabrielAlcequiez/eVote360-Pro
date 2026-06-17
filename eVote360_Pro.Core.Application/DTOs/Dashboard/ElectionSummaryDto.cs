using System;

namespace eVote360_Pro.Core.Application.DTOs.Dashboard
{
    public class ElectionSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int ParticipatingPartiesCount { get; set; }
        public int ParticipatingCandidatesCount { get; set; }
        public int VotedCitizensCount { get; set; }
    }
}
