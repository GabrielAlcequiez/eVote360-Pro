using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Domain.Entities
{
    public class CandidateOfficeAssignment
    {
        public Guid Id { get; private set; }

        public Guid CandidateId { get; private set; }
        public Candidate Candidate { get; private set; } = null!;

        public Guid ElectedOfficeId { get; private set; }
        public ElectedOffice ElectedOffice { get; private set; } = null!;

        // Partido que realiza la postulación (x defecto debe ser el partido del dirigente politico)
        public Guid PoliticalPartyId { get; private set; }
        public PoliticalParty PoliticalParty { get; private set; } = null!;

        public CandidacyType Type { get; private set; } // Propio o Aliado
        public DateTime CreatedAt { get; private set; } // No necesario, pero util


        protected CandidateOfficeAssignment() { }
        public CandidateOfficeAssignment(Guid electedOfficeId, Guid candidateId, Guid politicalPartyId, Guid candidateOwnerPartyId)
        {
            Id = Guid.NewGuid();
            ElectedOfficeId = electedOfficeId;
            CandidateId = candidateId;
            PoliticalPartyId = politicalPartyId;
            CreatedAt = DateTime.UtcNow; 

            Type = (politicalPartyId == candidateOwnerPartyId) ? CandidacyType.Standard : CandidacyType.Alliance;
        }
    }
}