namespace eVote360_Pro.Core.Domain.Entities
{
    // Esta entidad es simplemente para marcar que (equis) ciudadano haya votado
    public class CitizenParticipation
    {
        public Guid Id { get; private set; }
        
        public Guid ElectionId { get; private set; } 
        public Election Election { get; private set; } = null!;

        public Guid CitizenId { get; private set; } 
        public Citizen Citizen { get; private set; } = null!;

        protected CitizenParticipation() { }

        public CitizenParticipation(Guid electionId, Guid citizenId)
        {
            Id = Guid.NewGuid();
            ElectionId = electionId;
            CitizenId = citizenId;
        }
    }
}