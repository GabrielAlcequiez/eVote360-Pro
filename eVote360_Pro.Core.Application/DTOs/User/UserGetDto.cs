using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.User
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{Name} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        // La contraseña NO se expone en el DTO de consulta
        public Role Role { get; set; }
        public string RoleName => Role.ToString();
        public bool IsActive { get; set; }
    }
}
