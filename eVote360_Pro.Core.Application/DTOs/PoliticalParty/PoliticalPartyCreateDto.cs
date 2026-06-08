namespace eVote360_Pro.Core.Application.DTOs.PoliticalParty
{
    public class PoliticalPartyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Se almacenará siempre en mayúsculas (transformado en el servicio)
        public string Acronym { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
    }
}
