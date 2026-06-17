using System;
using System.Collections.Generic;
using eVote360_Pro.Core.Application.DTOs.Election;

namespace eVote360_Pro.Core.Application.DTOs.Dashboard
{
    public class DashboardGetDto
    {
        public int TotalCitizens { get; set; }
        public int ActiveCitizens { get; set; }
        public int InactiveCitizens { get; set; }
        
        public int TotalParties { get; set; }
        public int TotalCandidates { get; set; }
        public int TotalElectedOffices { get; set; }
        
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }

        public string? ActiveElectionName { get; set; }
        public Guid? ActiveElectionId { get; set; }
        public DateTime? ActiveElectionDate { get; set; }

        // Resultados de la elección activa o de la última finalizada
        public string? ResultsElectionName { get; set; }
        public string? ResultsElectionStatus { get; set; }
        public int VotedCitizensCount { get; set; }
        public decimal VotingParticipationPercentage { get; set; }
        public List<OfficeResultDto> ElectionResults { get; set; } = new();
    }
}
