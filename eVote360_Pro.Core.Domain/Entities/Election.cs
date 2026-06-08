using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Domain.Entities
{
    public class Election
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public DateTime ScheduledDate { get; private set; }
        public ElectionStatus Status { get; private set; }

        protected Election() { }

        public Election(string name, DateTime scheduledDate)
        {     
            Id = Guid.NewGuid();
            Name = name.Trim();
            ScheduledDate = scheduledDate;
            Status = ElectionStatus.Pending; 
        }

        public void Activate()
        {
            if (Status != ElectionStatus.Pending)
                throw new InvalidOperationException("Solo se puede activar una elección que esté en estado Pendiente.");

            Status = ElectionStatus.Active;
        }

        public void FinalizeElection()
        {
            if (Status != ElectionStatus.Active)
                throw new InvalidOperationException("Solo se puede finalizar una elección que esté actualmente Activa.");

            Status = ElectionStatus.Finalized;
        }
    }
}