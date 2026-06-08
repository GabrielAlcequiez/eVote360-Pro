using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.PoliticalAlliance
{
    public class PoliticalAllianceGetDto
    {
        public Guid Id { get; set; }

        // Partido que solicitó la alianza
        public Guid RequesterPartyId { get; set; }
        public string RequesterPartyName { get; set; } = string.Empty;
        public string RequesterPartyAcronym { get; set; } = string.Empty;

        // Partido que recibió la solicitud de alianza
        public Guid ReceiverPartyId { get; set; }
        public string ReceiverPartyName { get; set; } = string.Empty;
        public string ReceiverPartyAcronym { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public AllianceStatus Status { get; set; }
        public string StatusName => Status.ToString();
    }
}
