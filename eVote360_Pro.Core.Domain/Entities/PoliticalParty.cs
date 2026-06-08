namespace eVote360_Pro.Core.Domain.Entities
{
    public class PoliticalParty
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string Acronym { get; private set; } = string.Empty;
        public string Logo { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        protected PoliticalParty(){}
        public PoliticalParty(string name, string? description, string acronym, string logo)
        {
            Name = name.Trim();
            Description = description;
            Acronym = acronym.Trim().ToUpperInvariant();
            Logo = logo.Trim();
            IsActive = true;
        }

        public void Update(string name, string? description, string acronym, string logo, bool isActive)
        {
            Name = name.Trim();
            Description = description;
            Acronym = acronym.Trim().ToUpperInvariant();
            Logo = logo.Trim();
            IsActive = isActive;
        }
    }
}