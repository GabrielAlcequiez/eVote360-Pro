using System.Security.Claims;
using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.CandidateOfficeAssignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "PoliticalLeader")]
    public class CandidateOfficeAssignmentController : Controller
    {
        private readonly ICandidateOfficeAssignmentService _service;
        private readonly IElectionStatusService _electionStatus;
        private readonly IPartyLeaderService _partyLeaderService;
        private readonly IMapper _mapper;

        public CandidateOfficeAssignmentController(
            ICandidateOfficeAssignmentService service,
            IElectionStatusService electionStatus,
            IPartyLeaderService partyLeaderService,
            IMapper mapper)
        {
            _service = service;
            _electionStatus = electionStatus;
            _partyLeaderService = partyLeaderService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("AccessDenied", "Account");

            var assignments = await _service.GetAllAssignmentsAsync(userId.Value);
            var vm = _mapper.Map<List<CandidateOfficeAssignmentGetViewModel>>(assignments.ToList());
            ViewBag.HasActiveElection = await _electionStatus.HasActiveElectionAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await _electionStatus.HasActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede asignar candidatos a puestos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            var vm = new CandidateOfficeAssignmentCreateViewModel();
            await PopulateSelectListsAsync(vm, currentPartyId.Value);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateOfficeAssignmentCreateViewModel vm)
        {
            if (await _electionStatus.HasActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede asignar candidatos a puestos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync(vm, currentPartyId.Value);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<CandidateOfficeAssignmentCreateDto>(vm);
                dto.PoliticalPartyId = currentPartyId.Value;

                var userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("AccessDenied", "Account");

                await _service.AddAssignment(dto, userId.Value);
                TempData["SuccessMessage"] = "Asignación creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                await PopulateSelectListsAsync(vm, currentPartyId.Value);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateSelectListsAsync(vm, currentPartyId.Value);
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                var dto = await _service.GetById(id, userId.Value);
                var vm = _mapper.Map<CandidateOfficeAssignmentGetViewModel>(dto);
                return View(vm);
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "La asignación seleccionada no existe o ya fue eliminada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                await _service.DeleteAssignment(id, userId.Value);
                TempData["SuccessMessage"] = "Asignación eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return null;
            return userId;
        }

        private async Task<Guid?> GetCurrentPartyIdAsync()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return null;

            var leader = await _partyLeaderService.GetByUserIdAsync(userId.Value);
            return leader?.PoliticalPartyId;
        }

        private async Task PopulateSelectListsAsync(CandidateOfficeAssignmentCreateViewModel vm, Guid partyId)
        {
            var candidates = await _service.GetAvailableCandidatesAsync(partyId);
            vm.AvailableCandidates = candidates.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.FullName} - {c.PoliticalPartyName} ({c.PoliticalPartyAcronym})"
            }).ToList();

            var offices = await _service.GetAvailableOfficesAsync(partyId);
            vm.AvailableOffices = offices.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Name
            }).ToList();
        }
        #endregion
    }
}
