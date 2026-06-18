using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using eVote360_Pro.Core.Application.DTOs.PartyLeader;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.PartyLeader;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PartyLeaderController : Controller
    {
        private readonly IPartyLeaderService _partyLeaderService;
        private readonly IElectionStatusService _electionStatus;
        private readonly IMapper _mapper;

        public PartyLeaderController(
            IPartyLeaderService partyLeaderService,
            IElectionStatusService electionStatus,
            IMapper mapper)
        {
            _partyLeaderService = partyLeaderService;
            _electionStatus = electionStatus;
            _mapper = mapper;
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionStatus.HasActiveElectionAsync();
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _partyLeaderService.GetAllAsync();
            var hasActiveElection = await IsElectionActiveAsync();

            var vm = new PartyLeaderIndexViewModel
            {
                Assignments = assignments,
                HasActiveElection = hasActiveElection
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede asignar un dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new PartyLeaderCreateViewModel();
            await PopulateAvailableSelectionsAsync(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartyLeaderCreateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede asignar un dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await PopulateAvailableSelectionsAsync(vm);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<PartyLeaderCreateDto>(vm);
                await _partyLeaderService.AssignAsync(dto);
                TempData["SuccessMessage"] = "Dirigente político asignado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            await PopulateAvailableSelectionsAsync(vm);
            return View(vm);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede eliminar una asignación de dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var assignments = await _partyLeaderService.GetAllAsync();
            var assignment = assignments.FirstOrDefault(x => x.UserId == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede desvincular un dirigente político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _partyLeaderService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Asignación eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAvailableSelectionsAsync(PartyLeaderCreateViewModel vm)
        {
            var users = await _partyLeaderService.GetAvailableLeadersAsync();
            var parties = await _partyLeaderService.GetAvailablePartiesAsync();

            vm.AvailableLeaders = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.Name} {u.LastName} ({u.Username})"
            }).ToList();

            vm.AvailableParties = parties.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Acronym})"
            }).ToList();
        }
    }
}