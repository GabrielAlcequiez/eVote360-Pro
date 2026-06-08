using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.User
{
    public class UserCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
