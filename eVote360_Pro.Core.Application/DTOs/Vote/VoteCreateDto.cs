namespace eVote360_Pro.Core.Application.DTOs.Vote
{
    public class VoteCreateDto
    {
        public Guid ElectionId { get; set; }
        public Guid ElectedOfficeId { get; set; }

        // Es nullable porque el elector puede seleccionar "Ninguno" (voto en blanco)
        public Guid? CandidateId { get; set; }
    }
}
