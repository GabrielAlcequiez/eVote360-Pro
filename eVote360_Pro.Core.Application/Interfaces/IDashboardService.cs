using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.Dashboard;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene todas las métricas de control y resultados electorales necesarios para el dashboard administrativo.
        /// </summary>
        Task<DashboardGetDto> GetDashboardDataAsync(int? year = null);

        /// <summary>
        /// Obtiene la información del partido e indicadores principales para el home del dirigente político.
        /// </summary>
        Task<LeaderDashboardGetDto> GetLeaderDashboardDataAsync(Guid userId);
    }
}
