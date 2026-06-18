using System.Security.Claims;
using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.PoliticalAlliance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "PoliticalLeader")]
    public class PoliticalAllianceController(IPoliticalAllianceService politicalAllianceService, IElectionStatusService electionStatus, IPartyLeaderService partyLeaderService) : Controller
    {
        private readonly IPoliticalAllianceService _politicalAllianceService = politicalAllianceService;
        private readonly IElectionStatusService _electionStatus = electionStatus;
        private readonly IPartyLeaderService _partyLeaderService = partyLeaderService;

        public async Task<IActionResult> Index()
        {
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            var vm = new PoliticalAllianceIndexViewModel
            {
                PendingRequest = await _politicalAllianceService.GetPendingRequestAsync(currentPartyId.Value),
                SentRequests = await _politicalAllianceService.GetSentRequestAsync(currentPartyId.Value),
                ActiveAlliance = await _politicalAllianceService.GetActiveAllianceAsync(currentPartyId.Value),
                HasActiveElection = await _electionStatus.HasActiveElectionAsync()
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await _electionStatus.HasActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            var vm = new PoliticalAllianceCreateViewModel();
            await PopulateAvailablePartiesAsync(vm, currentPartyId.Value);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PoliticalAllianceCreateViewModel vm)
        {
            if (await _electionStatus.HasActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid)
            {
                await PopulateAvailablePartiesAsync(vm, currentPartyId.Value);
                return View(vm);
            }
            try
            {
                var dto = new PoliticalAllianceCreateDto { ReceiverPartyId = vm.ReceiverPartyId };
                await _politicalAllianceService.CreateRequestAsync(currentPartyId.Value, dto);
                TempData["SuccessMessage"] = "Solicitud de alianza enviada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateAvailablePartiesAsync(vm, currentPartyId.Value);
                return View(vm);
            }
        }


        public async Task<IActionResult> Accept(Guid id)
        {
            var alliance = await _politicalAllianceService.GetByIdAsync(id);
            if (alliance == null) return NotFound();
            return View(alliance);
        }

        [HttpPost]
        [ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptConfirmed(Guid id)
        {
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                await _politicalAllianceService.AcceptRequestAsync(id, currentPartyId.Value);
                TempData["SuccessMessage"] = "Solicitud de alianza aceptada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            var alliance = await _politicalAllianceService.GetByIdAsync(id);
            if (alliance == null) return NotFound();
            return View(alliance);
        }

        [HttpPost]
        [ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(Guid id)
        {
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                await _politicalAllianceService.RejectRequestAsync(id, currentPartyId.Value);
                TempData["SuccessMessage"] = "Solicitud de alianza rechazada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            var alliance = await _politicalAllianceService.GetByIdAsync(id);
            if (alliance == null) return NotFound();
            return View(alliance);
        }

        [HttpPost]
        [ActionName("DeleteRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRequestConfirmed(Guid id)
        {
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                await _politicalAllianceService.DeleteRequestAsync(id, currentPartyId.Value);
                TempData["SuccessMessage"] = "Solicitud de alianza eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAlliance(Guid id)
        {
            var alliance = await _politicalAllianceService.GetByIdAsync(id);
            if (alliance == null) return NotFound();
            return View(alliance);
        }

        [HttpPost]
        [ActionName("DeleteAlliance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllianceConfirmed(Guid id)
        {
            var currentPartyId = await GetCurrentPartyIdAsync();
            if (currentPartyId == null)
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                await _politicalAllianceService.DeleteAllianceAsync(id, currentPartyId.Value);
                TempData["SuccessMessage"] = "Alianza política eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private async Task<Guid?> GetCurrentPartyIdAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
                return null;

            var leader = await _partyLeaderService.GetByUserIdAsync(userId);
            return leader?.PoliticalPartyId;
        }

        private async Task PopulateAvailablePartiesAsync(PoliticalAllianceCreateViewModel vm, Guid currentPartyId)
        {
            var parties = await _politicalAllianceService.GetAvailablePartiesForAllAllianceAsync(currentPartyId);
            vm.AvailableParties = parties.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Acronym})"
            }).ToList();
        }
        #endregion
    }
}