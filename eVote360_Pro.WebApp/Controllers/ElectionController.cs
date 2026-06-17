using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.Election;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.Election;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ElectionController : Controller
    {
        private readonly IElectionService _electionService;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;

        public ElectionController(
            IElectionService electionService,
            IElectionRepository electionRepository,
            IMapper mapper)
        {
            _electionService = electionService;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionRepository.ValidateNoActiveElectionAsync();
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _electionService.GetAllAsync();
            var vm = _mapper.Map<IReadOnlyList<ElectionGetViewModel>>(dtos);
            ViewBag.HasActiveElection = await IsElectionActiveAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear una nueva elección mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new ElectionCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElectionCreateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear una nueva elección mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var createDto = _mapper.Map<ElectionCreateDto>(vm);
                await _electionService.AddElection(createDto);
                TempData["SuccessMessage"] = "Elección creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Activate(Guid id)
        {
            var dto = await _electionService.GetByIdAsync(id);
            if (dto is null)
                return NotFound();

            if (dto.Status != ElectionStatus.Pending)
            {
                TempData["ErrorMessage"] = "Solo se pueden activar elecciones en estado pendiente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ElectionName = dto.Name;
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(Guid id)
        {
            try
            {
                await _electionService.ActivateElection(id);
                TempData["SuccessMessage"] = "Elección activada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Finish(Guid id)
        {
            var dto = await _electionService.GetByIdAsync(id);
            if (dto is null)
                return NotFound();

            if (dto.Status != ElectionStatus.Active)
            {
                TempData["ErrorMessage"] = "Solo se pueden finalizar elecciones activas.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ElectionName = dto.Name;
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishConfirmed(Guid id)
        {
            try
            {
                await _electionService.FinishElection(id);
                TempData["SuccessMessage"] = "Elección finalizada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Results(Guid id)
        {
            var dto = await _electionService.GetByIdAsync(id);
            if (dto is null)
                return NotFound();

            if (dto.Status != ElectionStatus.Finalized)
            {
                TempData["ErrorMessage"] = "Solo se pueden consultar los resultados de elecciones finalizadas.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ElectionName = dto.Name;
            return View();
        }
    }
}
