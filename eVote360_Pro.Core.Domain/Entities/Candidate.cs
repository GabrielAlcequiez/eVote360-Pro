namespace eVote360_Pro.Core.Domain.Entities
{
    public class Candidate
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Photo { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        // Relación Obligatoria: Partido Político
        public Guid PoliticalPartyId { get; private set; }
        public PoliticalParty PoliticalParty { get; private set; } = null!;

        protected Candidate() { }
        public Candidate(string name, string lastName, string photo)
        {
            Id = Guid.NewGuid();
            Name = name;
            LastName = lastName;
            Photo = photo;
            IsActive = true;
        }

        public void Update(string name, string lastName, string photo, bool isActive)
        {
            Name = name;
            LastName = lastName;
            Photo = photo;
            IsActive = isActive;
        }
    }
}