using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.Ballot;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.Ballot;

namespace eVote360_Pro.WebApp.Controllers
{
    public class BallotController : Controller
    {
        private readonly IBallotService _ballotService;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;

        private const string SessionKeyCitizenId = "Voter_CitizenId";
        private const string SessionKeyCitizenName = "Voter_CitizenName";
        private const string SessionKeyCitizenEmail = "Voter_CitizenEmail";
        private const string SessionKeyElectionId = "Voter_ElectionId";
        private const string SessionKeyOcrValidated = "Voter_OcrValidated";
        private const string SessionKeyOtpValidated = "Voter_OtpValidated";
        private const string SessionKeySelections = "Ballot_Selections";

        public BallotController(IBallotService ballotService, IElectionRepository electionRepository, IMapper mapper)
        {
            _ballotService = ballotService;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        private bool IsSessionValid()
        {
            var citizenId = HttpContext.Session.GetString(SessionKeyCitizenId);
            var electionId = HttpContext.Session.GetString(SessionKeyElectionId);
            var ocrValid = HttpContext.Session.GetString(SessionKeyOcrValidated);
            var otpValid = HttpContext.Session.GetString(SessionKeyOtpValidated);

            return !string.IsNullOrEmpty(citizenId)
                && !string.IsNullOrEmpty(electionId)
                && ocrValid == "true"
                && otpValid == "true";
        }

        private Dictionary<Guid, Guid?> GetSelections()
        {
            var json = HttpContext.Session.GetString(SessionKeySelections);
            if (string.IsNullOrEmpty(json))
                return new Dictionary<Guid, Guid?>();

            return JsonSerializer.Deserialize<Dictionary<Guid, Guid?>>(json) ?? new Dictionary<Guid, Guid?>();
        }

        private void SaveSelections(Dictionary<Guid, Guid?> selections)
        {
            var json = JsonSerializer.Serialize(selections);
            HttpContext.Session.SetString(SessionKeySelections, json);
        }

        public async Task<IActionResult> Index()
        {
            if (!IsSessionValid())
            {
                TempData["ErrorMessage"] = "Debe completar el proceso de validación de identidad primero.";
                return RedirectToAction("Index", "Voter");
            }

            var electionId = Guid.Parse(HttpContext.Session.GetString(SessionKeyElectionId)!);
            var offices = await _ballotService.GetOfficesWithCandidatesAsync(electionId);
            var selections = GetSelections();

            var vm = _mapper.Map<List<OfficeViewModel>>(offices);
            foreach (var office in vm)
            {
                office.HasSelection = selections.ContainsKey(office.OfficeId);
            }

            ViewBag.AllSelected = offices.All(o => selections.ContainsKey(o.OfficeId));

            return View(vm);
        }

        public async Task<IActionResult> VoteForOffice(Guid id)
        {
            if (!IsSessionValid())
            {
                TempData["ErrorMessage"] = "Debe completar el proceso de validación de identidad primero.";
                return RedirectToAction("Index", "Voter");
            }

            var electionId = Guid.Parse(HttpContext.Session.GetString(SessionKeyElectionId)!);
            var offices = await _ballotService.GetOfficesWithCandidatesAsync(electionId);
            var office = offices.FirstOrDefault(o => o.OfficeId == id);

            if (office is null)
                return NotFound();

            var selections = GetSelections();
            var currentSelection = selections.GetValueOrDefault(id);

            var vm = new VoteCandidateViewModel
            {
                OfficeId = office.OfficeId,
                OfficeName = office.OfficeName,
                Candidates = _mapper.Map<List<CandidateOptionViewModel>>(office.Candidates),
                SelectedCandidateId = currentSelection
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VoteForOffice(Guid id, VoteCandidateViewModel vm)
        {
            if (!IsSessionValid())
            {
                TempData["ErrorMessage"] = "Debe completar el proceso de validación de identidad primero.";
                return RedirectToAction("Index", "Voter");
            }

            var selections = GetSelections();
            selections[id] = vm.SelectedCandidateId;
            SaveSelections(selections);

            TempData["SuccessMessage"] = "Selección guardada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalize()
        {
            if (!IsSessionValid())
            {
                TempData["ErrorMessage"] = "Debe completar el proceso de validación de identidad primero.";
                return RedirectToAction("Index", "Voter");
            }

            var electionId = Guid.Parse(HttpContext.Session.GetString(SessionKeyElectionId)!);
            var citizenId = Guid.Parse(HttpContext.Session.GetString(SessionKeyCitizenId)!);
            var citizenName = HttpContext.Session.GetString(SessionKeyCitizenName)!;
            var citizenEmail = HttpContext.Session.GetString(SessionKeyCitizenEmail)!;

            var offices = await _ballotService.GetOfficesWithCandidatesAsync(electionId);
            var selections = GetSelections();

            if (offices.Any(o => !selections.ContainsKey(o.OfficeId)))
            {
                var pending = offices
                    .Where(o => !selections.ContainsKey(o.OfficeId))
                    .Select(o => o.OfficeName);

                TempData["ErrorMessage"] = $"Debe completar su selección para los siguientes puestos electivos: {string.Join(", ", pending)}.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var election = await _electionRepository.GetByIdAsync(electionId);
                var electionName = election?.Name ?? "Elección";
                var electionDate = election?.ScheduledDate ?? DateTime.Today;

                await _ballotService.FinalizeVotingAsync(citizenId, electionId, selections, citizenName, citizenEmail, electionName, electionDate);

                ClearBallotSession();
                TempData["SuccessMessage"] = "Su voto ha sido registrado exitosamente. Recibirá un resumen por correo electrónico.";
                return RedirectToAction("Index", "Voter");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al finalizar la votación: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private void ClearBallotSession()
        {
            HttpContext.Session.Remove(SessionKeySelections);
        }
    }
}
