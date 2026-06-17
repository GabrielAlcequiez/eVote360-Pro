using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.Dashboard;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene todas las métricas de control y resultados electorales necesarios para el dashboard administrativo.
        /// </summary>
        Task<DashboardGetDto> GetDashboardDataAsync();
    }
}
