using System;
using System.IO;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.WebApp.Models.Voter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVote360_Pro.WebApp.Controllers
{
    [AllowAnonymous]
    public class VoterController(IVoterAuthService voterAuthService) : Controller
    {
        private readonly IVoterAuthService _voterAuthService = voterAuthService;

        private const string SessionKeyCitizenId = "Voter_CitizenId";
        private const string SessionKeyCitizenName = "Voter_CitizenName";
        private const string SessionKeyCitizenEmail = "Voter_CitizenEmail";
        private const string SessionKeyElectionId = "Voter_ElectionId";
        private const string SessionKeyOcrValidated = "Voter_OcrValidated";
        private const string SessionKeyOtpValidated = "Voter_OtpValidated";
        private const string SessionKeyDocumentNumber = "Voter_DocumentNumber";

        // Paso 1: Ingreso de Cédula (GET)
        public IActionResult Index()
        {
            // Limpiar cualquier sesión anterior del elector al cargar el inicio del flujo
            ClearVoterSession();
            return View(new VoterDocumentViewModel());
        }

        // Paso 1: Ingreso de Cédula (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(VoterDocumentViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var (citizen, election) = await _voterAuthService.VerifyCitizenEligibilityAsync(vm.DocumentNumber);

                // Guardar en sesión
                HttpContext.Session.SetString(SessionKeyCitizenId, citizen.Id.ToString());
                HttpContext.Session.SetString(SessionKeyCitizenName, $"{citizen.Name} {citizen.LastName}");
                HttpContext.Session.SetString(SessionKeyCitizenEmail, citizen.Email);
                HttpContext.Session.SetString(SessionKeyElectionId, election.Id.ToString());
                HttpContext.Session.SetString(SessionKeyDocumentNumber, citizen.DocumentNumber);
                HttpContext.Session.SetString(SessionKeyOcrValidated, "false");
                HttpContext.Session.SetString(SessionKeyOtpValidated, "false");

                return RedirectToAction(nameof(UploadOcr));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado: {ex.Message}");
                return View(vm);
            }
        }

        // Paso 2: Validación OCR (GET)
        public IActionResult UploadOcr()
        {
            var citizenIdStr = HttpContext.Session.GetString(SessionKeyCitizenId);
            var docNumber = HttpContext.Session.GetString(SessionKeyDocumentNumber);

            if (string.IsNullOrEmpty(citizenIdStr) || string.IsNullOrEmpty(docNumber))
            {
                TempData["ErrorMessage"] = "Debe ingresar su cédula primero.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new VoterOcrViewModel
            {
                DocumentNumber = docNumber
            };

            return View(vm);
        }

        // Paso 2: Validación OCR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadOcr(VoterOcrViewModel vm)
        {
            var citizenIdStr = HttpContext.Session.GetString(SessionKeyCitizenId);
            var docNumber = HttpContext.Session.GetString(SessionKeyDocumentNumber);

            if (string.IsNullOrEmpty(citizenIdStr) || string.IsNullOrEmpty(docNumber))
            {
                TempData["ErrorMessage"] = "Debe ingresar su cédula primero.";
                return RedirectToAction(nameof(Index));
            }

            // Forzar el número de documento de la sesión para evitar manipulaciones en el POST
            vm.DocumentNumber = docNumber;

            if (vm.DocumentImage == null || vm.DocumentImage.Length == 0)
            {
                ModelState.AddModelError(nameof(vm.DocumentImage), "Debe seleccionar o arrastrar una foto válida de su cédula.");
                return View(vm);
            }

            try
            {
                bool isOcrValid;
                using (var stream = vm.DocumentImage.OpenReadStream())
                {
                    isOcrValid = await _voterAuthService.VerifyDocumentOcrAsync(docNumber, stream);
                }

                if (!isOcrValid)
                {
                    ModelState.AddModelError(string.Empty, "La validación OCR falló. El número de cédula extraído de la imagen no coincide con el registrado. Intente con una foto más clara y legible.");
                    return View(vm);
                }

                // Validación OCR correcta
                HttpContext.Session.SetString(SessionKeyOcrValidated, "true");

                // Redirigir al envío del OTP
                return RedirectToAction(nameof(SendOtp));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error durante el procesamiento OCR: {ex.Message}");
                return View(vm);
            }
        }

        // Envío automático de OTP y redirección (GET)
        public async Task<IActionResult> SendOtp()
        {
            var citizenIdStr = HttpContext.Session.GetString(SessionKeyCitizenId);
            var name = HttpContext.Session.GetString(SessionKeyCitizenName);
            var email = HttpContext.Session.GetString(SessionKeyCitizenEmail);
            var electionIdStr = HttpContext.Session.GetString(SessionKeyElectionId);
            var ocrValid = HttpContext.Session.GetString(SessionKeyOcrValidated);

            if (string.IsNullOrEmpty(citizenIdStr) || ocrValid != "true")
            {
                TempData["ErrorMessage"] = "Debe completar la validación OCR de la cédula primero.";
                return RedirectToAction(nameof(UploadOcr));
            }

            try
            {
                var citizenId = Guid.Parse(citizenIdStr);
                var electionId = Guid.Parse(electionIdStr!);

                await _voterAuthService.GenerateAndSendOtpAsync(citizenId, electionId, email!, name!);
                TempData["SuccessMessage"] = "Se ha enviado un código OTP de 6 dígitos a su correo electrónico registrado.";
                return RedirectToAction(nameof(VerifyOtp));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"No se pudo enviar el código OTP: {ex.Message}";
                return RedirectToAction(nameof(UploadOcr));
            }
        }

        // Paso 3: Verificación OTP (GET)
        public IActionResult VerifyOtp()
        {
            var citizenIdStr = HttpContext.Session.GetString(SessionKeyCitizenId);
            var ocrValid = HttpContext.Session.GetString(SessionKeyOcrValidated);
            var email = HttpContext.Session.GetString(SessionKeyCitizenEmail);

            if (string.IsNullOrEmpty(citizenIdStr) || ocrValid != "true")
            {
                TempData["ErrorMessage"] = "Debe completar la validación OCR primero.";
                return RedirectToAction(nameof(UploadOcr));
            }

            // Ocultar parcialmente el correo por privacidad (ej: j***e@gmail.com)
            ViewBag.MaskedEmail = MaskEmail(email!);
            return View(new VoterOtpViewModel());
        }

        // Paso 3: Verificación OTP (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VoterOtpViewModel vm)
        {
            var citizenIdStr = HttpContext.Session.GetString(SessionKeyCitizenId);
            var electionIdStr = HttpContext.Session.GetString(SessionKeyElectionId);
            var ocrValid = HttpContext.Session.GetString(SessionKeyOcrValidated);
            var email = HttpContext.Session.GetString(SessionKeyCitizenEmail);

            if (string.IsNullOrEmpty(citizenIdStr) || ocrValid != "true")
            {
                TempData["ErrorMessage"] = "Debe completar la validación OCR primero.";
                return RedirectToAction(nameof(UploadOcr));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MaskedEmail = MaskEmail(email!);
                return View(vm);
            }

            try
            {
                var citizenId = Guid.Parse(citizenIdStr);
                var electionId = Guid.Parse(electionIdStr!);

                var isValid = await _voterAuthService.ValidateOtpAsync(citizenId, electionId, vm.OtpCode);

                if (isValid)
                {
                    HttpContext.Session.SetString(SessionKeyOtpValidated, "true");
                    TempData["SuccessMessage"] = "Identidad completamente validada. Bienvenido al proceso de votación.";
                    
                    // Redirigir a la boleta electoral de la Persona A
                    return RedirectToAction("Index", "Ballot");
                }

                ModelState.AddModelError(string.Empty, "Código OTP inválido.");
                ViewBag.MaskedEmail = MaskEmail(email!);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.MaskedEmail = MaskEmail(email!);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error de verificación: {ex.Message}");
                ViewBag.MaskedEmail = MaskEmail(email!);
                return View(vm);
            }
        }

        private void ClearVoterSession()
        {
            HttpContext.Session.Remove(SessionKeyCitizenId);
            HttpContext.Session.Remove(SessionKeyCitizenName);
            HttpContext.Session.Remove(SessionKeyCitizenEmail);
            HttpContext.Session.Remove(SessionKeyElectionId);
            HttpContext.Session.Remove(SessionKeyOcrValidated);
            HttpContext.Session.Remove(SessionKeyOtpValidated);
            HttpContext.Session.Remove(SessionKeyDocumentNumber);
        }

        private static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
                return email;

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 2)
                return $"{name[0]}*@{domain}";

            return $"{name[0]}{new string('*', name.Length - 2)}{name[^1]}@{domain}";
        }
    }
}
