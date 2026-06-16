using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IVerificationCodeRepository : IBaseRepository<VerificationCode>
    {
        /// <summary>
        /// Obtiene el último código de verificación activo y no utilizado para un ciudadano en una elección específica.
        /// </summary>
        Task<VerificationCode?> GetLatestUnusedCodeAsync(Guid citizenId, Guid electionId);
    }
}
