using System.Collections;

namespace eVote360_Pro.Core.Domain.Entities
{
    public class Citizen
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string DocumentNumber { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        protected Citizen() { }
        public Citizen(string name, string lastName, string email, string documentNumber)
        {
            Id = Guid.NewGuid();
            Name = name.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            DocumentNumber = documentNumber.Trim();
            IsActive = true;
        }

        public void Update(string name, string lastName, string email, string documentNumber, bool isActive)
        {
            Name = name.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            DocumentNumber = documentNumber.Trim();
            IsActive = isActive;
        }


    }
}