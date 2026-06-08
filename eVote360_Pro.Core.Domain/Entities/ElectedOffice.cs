namespace eVote360_Pro.Core.Domain.Entities
{
    public class ElectedOffice
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        protected ElectedOffice() { }
        public ElectedOffice(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name.Trim();
            Description = description.Trim();
            IsActive = true;
        }

        public void Update(string name, string description, bool isActive)
        {
            Name = name.Trim();
            Description = description.Trim();
            IsActive = isActive;
        }
    }
}