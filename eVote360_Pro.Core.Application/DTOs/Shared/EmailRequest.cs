namespace eVote360_Pro.Core.Application.DTOs.Shared
{
    public class EmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty; // Supports HTML strings
    }
}