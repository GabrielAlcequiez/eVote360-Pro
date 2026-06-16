using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.Shared;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class VoterAuthService : IVoterAuthService
    {
        private readonly ICitizenRepository _citizenRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly IVerificationCodeRepository _verificationCodeRepository;
        private readonly IOcrService _ocrService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public VoterAuthService(
            ICitizenRepository citizenRepository,
            IElectionRepository electionRepository,
            IVerificationCodeRepository verificationCodeRepository,
            IOcrService ocrService,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _citizenRepository = citizenRepository;
            _electionRepository = electionRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _ocrService = ocrService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<(Citizen Citizen, Election ActiveElection)> VerifyCitizenEligibilityAsync(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("El número de documento no puede estar vacío.");

            // Limpiamos los guiones para comparar con la BD
            var cleanDocument = documentNumber.Replace("-", "");
            var citizen = await _citizenRepository.GetByDocumentNumber(cleanDocument) ?? throw new InvalidOperationException("El ciudadano ingresado no se encuentra registrado.");

            if (!citizen.IsActive)
                throw new InvalidOperationException("El ciudadano ingresado se encuentra inactivo.");

            // Buscar si existe una elección activa
            var activeElection = await _electionRepository.GetActiveElectionAsync() ?? throw new InvalidOperationException("No hay ninguna elección activa en este momento.");

            // Verificar si el ciudadano ya votó en esta elección activa
            var alreadyVoted = await _citizenRepository.HasBeenUsedInElectionAsync(citizen.Id);
            if (alreadyVoted)
                throw new InvalidOperationException("El ciudadano ya ha ejercido su voto en esta elección.");

            return (citizen, activeElection);
        }

        public async Task<bool> VerifyDocumentOcrAsync(string enteredDocumentNumber, Stream imageStream)
        {
            if (string.IsNullOrWhiteSpace(enteredDocumentNumber))
                throw new ArgumentException("El número de documento ingresado es inválido.");

            var cleanEntered = enteredDocumentNumber.Replace("-", "");

            // Extraer el texto de la cédula mediante OCR
            var extractedDocument = await _ocrService.ExtractDocumentNumberAsync(imageStream);

            if (string.IsNullOrWhiteSpace(extractedDocument))
                return false;

            var cleanExtracted = extractedDocument.Replace("-", "");
            // Verificar si el texto extraído (limpio) contiene la cédula ingresada (limpia)
            return cleanExtracted.Contains(cleanEntered);
        }

        public async Task<bool> GenerateAndSendOtpAsync(Guid citizenId, Guid electionId, string email, string name)
        {
            // Generar código OTP de 6 dígitos aleatorio y seguro
            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Crear entidad de código (vigencia de 5 minutos configurada internamente)
            var verificationCode = new VerificationCode(citizenId, electionId, code);

            // Persistir en BD
            await _verificationCodeRepository.AddAsync(verificationCode);
            await _unitOfWork.CompleteAsync();

            // Enviar correo
            var emailRequest = new EmailRequest
            {
                ToEmail = email,
                RecipientName = name,
                Subject = "Código de Verificación OTP - eVote360 Pro",
                Body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                        <h2 style='color: #2b6cb0; text-align: center; font-size: 24px; margin-bottom: 5px;'>eVote360 Pro</h2>
                        <p style='text-align: center; color: #718096; font-size: 14px; margin-top: 0;'>Sistema Electoral Digital</p>
                        <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'/>
                        <p>Estimado/a <strong>{name}</strong>,</p>
                        <p>Usted ha solicitado ingresar al flujo de votación electrónica activa. Su código de verificación de un solo uso (OTP) es:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #2d3748; background-color: #edf2f7; padding: 12px 30px; border-radius: 6px; border: 1px solid #cbd5e0; display: inline-block;'>{code}</span>
                        </div>
                        <p style='color: #e53e3e; font-weight: bold; text-align: center;'>Este código expira en 5 minutos y solo puede ser utilizado una vez.</p>
                        <p style='font-size: 14px; color: #4a5568;'>Si usted no ha intentado acceder al sistema de votación, por favor ignore este mensaje.</p>
                        <hr style='border: 0; border-top: 1px solid #eee; margin: 30px 0;'/>
                        <p style='font-size: 12px; color: #a0aec0; text-align: center;'>Junta Central Electoral (JCE) &copy; {DateTime.Now.Year}</p>
                    </div>"
            };

            await _emailService.SendEmailAsync(emailRequest);
            return true;
        }

        public async Task<bool> ValidateOtpAsync(Guid citizenId, Guid electionId, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código no puede estar vacío.");

            // Obtener el último código no usado generado
            var verificationCode = await _verificationCodeRepository.GetLatestUnusedCodeAsync(citizenId, electionId);

            if (verificationCode == null)
                throw new InvalidOperationException("No se encontró ningún código de verificación activo.");

            // Validar que el código ingresado coincida
            if (!verificationCode.Code.Equals(code.Trim(), StringComparison.Ordinal))
                throw new InvalidOperationException("El código de verificación ingresado es incorrecto.");

            // Consumir el código (valida exp/uso)
            verificationCode.Use();

            // Persistir cambios
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
