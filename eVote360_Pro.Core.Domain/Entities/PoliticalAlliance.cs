using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Domain.Entities
{
    public class PoliticalAlliance
    {
        public Guid Id { get; private set; }

        public Guid RequesterPartyId { get; private set; } // fk
        public PoliticalParty RequesterParty { get; private set; } = null!; // navigation propertie de fk   
        public Guid ReceiverPartyId { get; private set; } // fk
        public PoliticalParty ReceiverParty { get; private set; } = null!;  // navigation propertie de fk   

        public DateTime RequestDate { get; private set; }
        public DateTime? AcceptanceDate { get; private set; }
        public AllianceStatus Status { get; private set; }

        protected PoliticalAlliance() { }
        public PoliticalAlliance(Guid requesterId, Guid receiverId)
        {
            Id = Guid.NewGuid();
            RequesterPartyId = requesterId;
            ReceiverPartyId = receiverId;
            RequestDate = DateTime.UtcNow;
            AcceptanceDate = null;
            Status = AllianceStatus.Pending;
        }

        // Método Update para la acción de Aceptar
        public void Accept()
        {
            if (Status != AllianceStatus.Pending)
                throw new InvalidOperationException("Esta solicitud de alianza ya fue respondida.");

            Status = AllianceStatus.Accepted;
            AcceptanceDate = DateTime.UtcNow;
        }

        // Método Update para la acción de Rechazar
        public void Reject()
        {
            if (Status != AllianceStatus.Pending)
                throw new InvalidOperationException("Esta solicitud de alianza ya fue respondida.");

            Status = AllianceStatus.Rejected;
            AcceptanceDate = null;
        }
    }
}