namespace eVote360_Pro.Core.Application.DTOs.Citizen
{
    public class CitizenUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
