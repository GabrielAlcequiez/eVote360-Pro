using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.Citizen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CitizenController : Controller
    {
        private readonly IElectionRepository _electionRepository;
        private readonly ICitizenService _service;
        private readonly IMapper _mapper;

        public CitizenController(IElectionRepository electionRepository, ICitizenService service, IMapper mapper)
        {
            _electionRepository = electionRepository;
            _service = service;
            _mapper = mapper;
        }
        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionRepository.ValidateNoActiveElectionAsync();
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var vms = _mapper.Map<IReadOnlyList<CitizenGetViewModel>>(dtos);
            ViewBag.HasActiveElection = await IsElectionActiveAsync();
            return View(vms);
        }

        public async Task<ActionResult> Create()
        {
             if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new CitizenCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitizenCreateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var createDto = _mapper.Map<CitizenCreateDto>(vm);
                await _service.AddAsync(createDto);
                TempData["SuccessMessage"] = "Ciudadano creado exitosamente.";
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
                TempData["ErrorMessage"] = "No se puede editar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var dto = await _service.GetByIdAsync(id);

            if (dto is null)
                return NotFound();

            var vm = _mapper.Map<CitizenUpdateViewModel>(dto);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CitizenUpdateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var updateDto = _mapper.Map<CitizenUpdateDto>(vm);
                updateDto.Id = id;
                await _service.UpdateAsync(updateDto);
                TempData["SuccessMessage"] = "Ciudadano actualizado exitosamente.";
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

        [HttpGet]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se pueden modificar ciudadanos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusPost(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se pueden modificar ciudadanos mientras exista una elección activa.";
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
                    TempData["SuccessMessage"] = "Ciudadano desactivado exitosamente.";
                }
                else
                {
                    var updateDto = _mapper.Map<CitizenUpdateDto>(dto);
                    updateDto.IsActive = true;
                    await _service.UpdateAsync(updateDto);
                    TempData["SuccessMessage"] = "Ciudadano activado exitosamente.";
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