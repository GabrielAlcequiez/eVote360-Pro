using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.User
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        // La contraseña es opcional en la actualización; null significa "no cambiar"
        public string? Password { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
    }
}
