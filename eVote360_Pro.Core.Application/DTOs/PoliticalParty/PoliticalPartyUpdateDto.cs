namespace eVote360_Pro.Core.Application.DTOs.PoliticalParty
{
    public class PoliticalPartyUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Se almacenará siempre en mayúsculas (transformado en el servicio)
        public string Acronym { get; set; } = string.Empty;

        // Logo es opcional en actualización; si es null no se modifica la imagen
        public string? Logo { get; set; }
        public bool IsActive { get; set; }
    }
}
