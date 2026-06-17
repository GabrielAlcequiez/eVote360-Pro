using System;

namespace eVote360_Pro.Core.Application.DTOs.Dashboard
{
    public class LeaderDashboardGetDto
    {
        public string PartyName { get; set; } = string.Empty;
        public string PartyAcronym { get; set; } = string.Empty;
        public string PartyLogo { get; set; } = string.Empty;
        
        public int ActiveCandidatesCount { get; set; }
        public int InactiveCandidatesCount { get; set; }
        public int AlliancesCount { get; set; }
        public int PendingAlliancesCount { get; set; }
        public int AssignedCandidatesCount { get; set; }
    }
}
