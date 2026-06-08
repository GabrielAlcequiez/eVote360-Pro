using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.Election
{
    public class ElectionGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public ElectionStatus Status { get; set; }
        public string StatusName => Status.ToString();
    }
}
