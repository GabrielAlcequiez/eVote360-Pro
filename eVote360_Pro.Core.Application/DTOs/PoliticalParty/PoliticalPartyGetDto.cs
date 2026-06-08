namespace eVote360_Pro.Core.Application.DTOs.PoliticalParty
{
    public class PoliticalPartyGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        // El acrónimo siempre se muestra en mayúsculas
        public string Acronym { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Información del dirigente (si tiene asignado)
        public string? LeaderFullName { get; set; }
    }
}
