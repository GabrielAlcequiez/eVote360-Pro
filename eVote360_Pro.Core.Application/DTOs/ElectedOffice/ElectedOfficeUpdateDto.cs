namespace eVote360_Pro.Core.Application.DTOs.ElectedOffice
{
    public class ElectedOfficeUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
