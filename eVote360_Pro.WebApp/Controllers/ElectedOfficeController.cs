using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.ViewModels.ElectedOffice;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ElectedOfficeController : Controller
    {
        private readonly IElectedOfficeService _service;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;

        public ElectedOfficeController(
            IElectedOfficeService service,
            IElectionRepository electionRepository,
            IMapper mapper)
        {
            _service = service;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionRepository.ValidateNoActiveElectionAsync();
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var vm = _mapper.Map<IReadOnlyList<ElectedOfficeGetViewModel>>(dtos);
            ViewBag.HasActiveElection = await IsElectionActiveAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new ElectedOfficeCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElectedOfficeCreateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var createDto = _mapper.Map<ElectedOfficeCreateDto>(vm);
                await _service.AddAsync(createDto);
                TempData["SuccessMessage"] = "Puesto electivo creado exitosamente.";
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

        public async Task<IActionResult> Edit(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var dto = await _service.GetByIdAsync(id);

            if (dto is null)
                return NotFound();

            var vm = _mapper.Map<ElectedOfficeUpdateViewModel>(dto);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ElectedOfficeUpdateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var updateDto = _mapper.Map<ElectedOfficeUpdateDto>(vm);
                updateDto.Id = id;
                await _service.UpdateAsync(updateDto);
                TempData["SuccessMessage"] = "Puesto electivo actualizado exitosamente.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se pueden modificar puestos electivos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                    return NotFound();

                if (dto.IsActive)
                {
                    await _service.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Puesto electivo desactivado exitosamente.";
                }
                else
                {
                    var updateDto = _mapper.Map<ElectedOfficeUpdateDto>(dto);
                    updateDto.IsActive = true;
                    await _service.UpdateAsync(updateDto);
                    TempData["SuccessMessage"] = "Puesto electivo activado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}