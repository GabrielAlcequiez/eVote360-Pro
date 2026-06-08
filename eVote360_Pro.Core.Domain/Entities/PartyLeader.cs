namespace eVote360_Pro.Core.Domain.Entities
{
    public class PartyLeader
    {
        public Guid UserId {get; private set;} // fk hacia User (usuario con dicho rol)
        public Guid PoliticalPartyId {get; private set;} // fk hacia partido politico

        // Navegation Properties
        public User User {get; private set;} = null!;
        public PoliticalParty PoliticalParty {get; private set;} = null!;

        protected PartyLeader(){}
    
        // Pendiente de revisar forma más efectiva
        public PartyLeader(Guid userId, Guid politicalPartyId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("El ID del dirigente no puede estar vacío.", nameof(userId));

            if (politicalPartyId == Guid.Empty)
                throw new ArgumentException("El ID del partido no puede estar vacío.", nameof(politicalPartyId));

            UserId = userId;
            PoliticalPartyId = politicalPartyId;
        }
    }
}