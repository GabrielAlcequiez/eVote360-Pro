using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.DTOs.Election
{
    public class ElectionUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }

        // El estado solo lo modifica el sistema mediante métodos de dominio (Activate/Finalize)
        // pero se incluye aquí para referencia en formularios de administración
        public ElectionStatus Status { get; set; }
    }
}
