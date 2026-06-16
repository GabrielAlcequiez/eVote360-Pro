using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment
{
    public class CandidateOfficeAssignmentGetDto
    {
        public Guid Id { get; set; }

        // Candidato
        public Guid CandidateId { get; set; }
        public string CandidateFullName { get; set; } = string.Empty;
        public string CandidatePhoto { get; set; } = string.Empty;

        // Cargo electivo
        public Guid ElectedOfficeId { get; set; }
        public string ElectedOfficeName { get; set; } = string.Empty;

        // Partido postulante
        public Guid PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = string.Empty;
        public string PoliticalPartyAcronym { get; set; } = string.Empty;

        // Partido de origen del candidato
        public string CandidateOriginPartyName { get; set; } = string.Empty;
        public string CandidateOriginPartyAcronym { get; set; } = string.Empty;

        // Tipo de candidatura: Propio o Aliado
        public CandidacyType Type { get; set; }
        public string TypeName => Type.ToString();

        public DateTime CreatedAt { get; set; }
    }
}
