using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Helpers;
using eVote360_Pro.WebApp.Models.PoliticalParty;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PoliticalPartyController : Controller
    {
        private readonly IPoliticalPartyService _partyService;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;

        public PoliticalPartyController(
            IPoliticalPartyService partyService,
            IElectionRepository electionRepository,
            IMapper mapper)
        {
            _partyService = partyService;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var parties = await _partyService.GetAllAsync();
            var hasActiveElection = await _electionRepository.ValidateNoActiveElectionAsync();

            var viewModel = new PoliticalPartyIndexViewModel
            {
                Parties = parties,
                HasActiveElection = hasActiveElection
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            return View(new PoliticalPartyCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PoliticalPartyCreateViewModel model)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 1. Crear partido con placeholder para obtener el ID
                var createDto = new PoliticalPartyCreateDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    Acronym = model.Acronym,
                    Logo = string.Empty
                };

                var createdParty = await _partyService.AddAsync(createDto);

                // 2. Subir logo con el ID generado
                var logoPath = FileManager.Upload(model.LogoFile, createdParty.Id, "Logos");

                if (string.IsNullOrEmpty(logoPath))
                {
                    ModelState.AddModelError("LogoFile", "El logo del partido debe ser una imagen válida (.jpg, .jpeg, .png).");
                    return View(model);
                }

                // 3. Actualizar el logo en la entidad
                var updateDto = new PoliticalPartyUpdateDto
                {
                    Id = createdParty.Id,
                    Name = createdParty.Name,
                    Description = createdParty.Description,
                    Acronym = createdParty.Acronym,
                    Logo = logoPath,
                    IsActive = createdParty.IsActive
                };

                await _partyService.UpdateAsync(updateDto);

                TempData["SuccessMessage"] = "Partido político creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var party = await _partyService.GetByIdAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            var isLocked = await _partyService.HasParticipatedInElectionAsync(id);

            var model = new PoliticalPartyUpdateViewModel
            {
                Id = party.Id,
                Name = party.Name,
                Description = party.Description,
                Acronym = party.Acronym,
                Logo = party.Logo,
                IsActive = party.IsActive,
                IsLocked = isLocked
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PoliticalPartyUpdateViewModel model)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            // Si está bloqueado, el navegador no envía Name ni Acronym. Los recuperamos del servicio para no perderlos.
            var existingParty = await _partyService.GetByIdAsync(model.Id);
            if (existingParty == null)
            {
                return NotFound();
            }

            var isLocked = await _partyService.HasParticipatedInElectionAsync(model.Id);
            model.IsLocked = isLocked;

            if (isLocked)
            {
                // Sobrescribimos con los valores originales de la DB para pasar validación y seguridad
                model.Name = existingParty.Name;
                model.Acronym = existingParty.Acronym;
                model.Logo = existingParty.Logo;
                
                // Removemos del model state errores relacionados con Name/Acronym ya que no los enviará el form
                ModelState.Remove(nameof(model.Name));
                ModelState.Remove(nameof(model.Acronym));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Si está bloqueado y se intenta subir un logo, rechazar
            if (isLocked && model.LogoFile != null)
            {
                ModelState.AddModelError(string.Empty, "No se puede modificar el logo de este partido político porque ya participó en una elección.");
                return View(model);
            }

            try
            {
                var logoPath = FileManager.Upload(model.LogoFile, model.Id, "Logos",
                    isEditMode: true, imagePath: model.Logo);

                if (model.LogoFile != null && string.IsNullOrEmpty(logoPath))
                {
                    ModelState.AddModelError("LogoFile", "El logo del partido debe ser una imagen válida (.jpg, .jpeg, .png).");
                    return View(model);
                }

                var dto = new PoliticalPartyUpdateDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Acronym = model.Acronym,
                    Logo = logoPath!, // FileManager retorna el path existente si no hay archivo nuevo
                    IsActive = model.IsActive
                };

                await _partyService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Partido político actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se pueden realizar cambios mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var party = await _partyService.GetByIdAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatusPost(Guid id)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se pueden realizar cambios mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var party = await _partyService.GetByIdAsync(id);
                if (party == null)
                {
                    return NotFound();
                }

                if (party.IsActive)
                {
                    // Desactivar: llama a DeleteAsync (que internamente hace el SoftDelete)
                    await _partyService.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Partido político desactivado exitosamente.";
                }
                else
                {
                    // Activar: usa UpdateAsync con IsActive = true
                    var dto = _mapper.Map<PoliticalPartyUpdateDto>(party);
                    dto.IsActive = true;
                    await _partyService.UpdateAsync(dto);
                    TempData["SuccessMessage"] = "Partido político activado exitosamente.";
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
