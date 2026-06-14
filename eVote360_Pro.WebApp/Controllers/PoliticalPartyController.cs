using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.PoliticalParty;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PoliticalPartyController : Controller
    {
        private readonly IPoliticalPartyService _partyService;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PoliticalPartyController(
            IPoliticalPartyService partyService,
            IElectionRepository electionRepository,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _partyService = partyService;
            _electionRepository = electionRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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

            // Validar que el archivo subido sea una imagen
            if (model.LogoFile != null && !IsValidImage(model.LogoFile))
            {
                ModelState.AddModelError("LogoFile", "El logo del partido debe ser una imagen válida (.jpg, .jpeg, .png).");
                return View(model);
            }

            try
            {
                string logoPath = string.Empty;
                if (model.LogoFile != null)
                {
                    logoPath = await UploadLogoAsync(model.LogoFile);
                }

                var dto = new PoliticalPartyCreateDto
                {
                    Name = model.Name,
                    Description = model.Description,
                    Acronym = model.Acronym,
                    Logo = logoPath
                };

                await _partyService.AddAsync(dto);
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

            // Validar que el archivo subido sea una imagen si se provee
            if (model.LogoFile != null)
            {
                if (isLocked)
                {
                    ModelState.AddModelError(string.Empty, "No se puede modificar el logo de este partido político porque ya participó en una elección.");
                    return View(model);
                }

                if (!IsValidImage(model.LogoFile))
                {
                    ModelState.AddModelError("LogoFile", "El logo del partido debe ser una imagen válida (.jpg, .jpeg, .png).");
                    return View(model);
                }
            }

            try
            {
                string? logoPath = null;
                if (model.LogoFile != null && !isLocked)
                {
                    logoPath = await UploadLogoAsync(model.LogoFile);
                }

                var dto = new PoliticalPartyUpdateDto
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Acronym = model.Acronym,
                    Logo = logoPath ?? model.Logo, // Si no se sube logo nuevo, se mantiene el actual
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

        #region Helper Methods
        private static bool IsValidImage(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }

        private async Task<string> UploadLogoAsync(IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logos");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/logos/{uniqueFileName}";
        }
        #endregion
    }
}
