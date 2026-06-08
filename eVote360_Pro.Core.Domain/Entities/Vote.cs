
namespace eVote360_Pro.Core.Domain.Entities
{
    public class Vote
    {
        public Guid Id { get; private set; }

        public Guid ElectionId { get; private set; }
        public Election Election { get; private set; } = null!;

        public Guid ElectedOfficeId { get; private set; } 
        public ElectedOffice ElectedOffice { get; private set; } = null!;

        // 
        public Guid? CandidateId { get; private set; }
        public Candidate? Candidate { get; private set; }

        protected Vote() { }

        public Vote(Guid electionId, Guid electedOfficeId, Guid? candidateId)
        {
            if (electionId == Guid.Empty || electedOfficeId == Guid.Empty)
                throw new ArgumentException("Los IDs de elección y puesto electivo son obligatorios.");

            Id = Guid.NewGuid();
            ElectionId = electionId;
            ElectedOfficeId = electedOfficeId;
            CandidateId = candidateId; // Puede ser null si el elector seleccionó "Ninguno"
        }
    }
}