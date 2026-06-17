using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.WebApp.Models.Election
{
    public class ElectionGetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public ElectionStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public int PartyCount { get; set; }
        public int OfficeCount { get; set; }
        public int VoterCount { get; set; }
    }
}
