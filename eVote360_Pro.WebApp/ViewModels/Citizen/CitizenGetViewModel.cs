namespace eVote360_Pro.WebApp.ViewModels.Citizen
{
    public class CitizenGetViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{Name} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
