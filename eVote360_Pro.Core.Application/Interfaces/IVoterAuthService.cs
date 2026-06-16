using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IVoterAuthService
    {
        /// <summary>
        /// Valida si el ciudadano existe, está activo, si hay una elección activa y si el ciudadano aún no ha participado.
        /// Retorna el ciudadano elegible y la elección activa. Lanza excepciones específicas si falla alguna validación.
        /// </summary>
        Task<(Citizen Citizen, Election ActiveElection)> VerifyCitizenEligibilityAsync(string documentNumber);

        /// <summary>
        /// Ejecuta el proceso de OCR sobre la imagen provista y valida si el número de cédula extraído coincide con el documento ingresado.
        /// </summary>
        Task<bool> VerifyDocumentOcrAsync(string enteredDocumentNumber, Stream imageStream);

        /// <summary>
        /// Genera un código OTP de 6 dígitos, lo persiste en base de datos y lo envía al correo del ciudadano.
        /// </summary>
        Task<bool> GenerateAndSendOtpAsync(Guid citizenId, Guid electionId, string email, string name);

        /// <summary>
        /// Valida el código OTP ingresado para la elección. Si es correcto, lo marca como utilizado en un solo paso.
        /// </summary>
        Task<bool> ValidateOtpAsync(Guid citizenId, Guid electionId, string code);
    }
}
