namespace eVote360_Pro.Core.Application.DTOs.Election
{
    public class ElectionCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
    }
}
