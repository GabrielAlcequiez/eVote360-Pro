using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;

namespace eVote360_Pro.WebApp.Models.PoliticalAlliance
{
    public class PoliticalAllianceIndexViewModel
    {
        public List<PoliticalAllianceGetDto> PendingRequest { get; set; } = [];
        public List<PoliticalAllianceGetDto> SentRequests { get; set; } = [];
        public List<PoliticalAllianceGetDto> ActiveAlliance { get; set; } = [];
        public bool HasActiveElection { get; set; }
    }
}