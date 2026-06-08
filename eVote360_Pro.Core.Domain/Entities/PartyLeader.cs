namespace eVote360_Pro.Core.Domain.Entities
{
    public class PartyLeader
    {
        public Guid PartyLeaderId {get; private set;} // fk hacia User (usuario con dicho rol)
        public Guid PoliticalPartyId {get; private set;} // fk hacia partido politico

        // Navegation Properties
        public User User {get; private set;} = null!;
        public PoliticalParty PoliticalParty {get; private set;} = null!;

        protected PartyLeader(){}
    
        // Pendiente de revisar forma más efectiva
        public PartyLeader(Guid partyLeaderId, Guid politicalPartyId)
        {
            if (partyLeaderId == Guid.Empty)
                throw new ArgumentException("El ID del dirigente no puede estar vacío.", nameof(partyLeaderId));

            if (politicalPartyId == Guid.Empty)
                throw new ArgumentException("El ID del partido no puede estar vacío.", nameof(politicalPartyId));

            PartyLeaderId = partyLeaderId;
            PoliticalPartyId = politicalPartyId;
        }
    }
}