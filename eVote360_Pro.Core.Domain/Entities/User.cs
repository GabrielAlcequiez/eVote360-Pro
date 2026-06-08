using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public Role Role { get; private set; }
        public bool IsActive { get; private set; }
        
        // Relación 1:1 - Un dirigente pertenece a un solo partido
        // Es nullable porque un Administrador no tendrá esta asignación
        public PartyLeader? PartyLeaderAssignment { get; private set; }

        protected User() { }
        public User(string name, string lastName, string email, string userName, string password, Role role)
        {
            Id = Guid.NewGuid();
            Name = name.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            Username = userName.Trim();
            Password = password;
            Role = role;
            IsActive = true;
        }

        public void Update(string name, string lastName, string email, string userName, string password, Role role, bool isActive)
        {
            Name = name.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            Username = userName.Trim();
            Password = password;
            Role = role;
            IsActive = isActive;
        }

    }
}