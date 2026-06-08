namespace eVote360_Pro.Core.Application.DTOs.VerificationCode
{
    public class VerificationCodeGetDto
    {
        public Guid Id { get; set; }
        public Guid CitizenId { get; set; }
        public string CitizenFullName { get; set; } = string.Empty;
        public Guid ElectionId { get; set; }
        public string ElectionName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
