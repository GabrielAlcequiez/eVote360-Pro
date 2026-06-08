namespace eVote360_Pro.Core.Application.DTOs.Candidate
{
    public class CandidateCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;

        // El partido político NO se selecciona en el formulario de creación;
        // se asigna automáticamente según el dirigente político autenticado.
    }
}
