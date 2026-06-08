namespace eVote360_Pro.Core.Application.DTOs.PoliticalAlliance
{
    public class PoliticalAllianceCreateDto
    {
        // El RequesterPartyId se asigna automáticamente desde el partido del dirigente autenticado.
        // Solo se necesita seleccionar el partido receptor de la solicitud.
        public Guid ReceiverPartyId { get; set; }
    }
}
