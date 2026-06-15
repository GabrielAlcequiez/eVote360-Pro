using System.Security.Claims;
using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Helpers;
using eVote360_Pro.WebApp.Models.Candidate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "PoliticalLeader")]
    public class CandidateController : Controller
    {

        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateService _service;
        private readonly IMapper _mapper;
        public CandidateController(IElectionRepository electionRepository, ICandidateService service, IMapper mapper)
        {
            _electionRepository = electionRepository;
            _service = service;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var vms = _mapper.Map<List<CandidateGetViewModel>>(dtos);
            ViewBag.HasActiveElection = await IsElectionActiveAsync();
            return View(vms);
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionRepository.ValidateNoActiveElectionAsync();
        }

        public async Task<ActionResult> Create()
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un candidato mientras exista una elección activa";
                return RedirectToAction(nameof(Index));
            }
            return View(new CandidateCreateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CandidateCreateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un candidato mientras exista una elección activa";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                // pendiente de investigar si es eficiente el ! en este caso
                // (se supone que la autenticación y autorización es correcta, si esta aqui es por que hay un usuario en sesión)
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var createdDto = _mapper.Map<CandidateCreateDto>(vm);
                var createdCandidate = await _service.AddAsync(createdDto, userId);

                var photoPath = FileManager.Upload(vm.Photo, createdCandidate.Id, "Candidates");

                if (string.IsNullOrEmpty(photoPath))
                {
                    ModelState.AddModelError("PhotoFile", "La foto del candidato debe ser una imagen válida (.jpg, .jpeg, .png).");
                    return View(vm);
                }

                var updateDto = _mapper.Map<CandidateUpdateDto>(createdCandidate);
                updateDto.Photo = photoPath;
                await _service.UpdateAsync(updateDto, userId);

                TempData["SuccessMessage"] = "Candidato creado exitosamente.";
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
                TempData["ErrorMessage"] = "No se puede crear un candidato mientras exista una elección activa";
                return RedirectToAction(nameof(Index));

            }
            var dto = await _service.GetByIdAsync(id);
            if (dto is null)
                return NotFound();

            var vm = _mapper.Map<CandidateUpdateViewModel>(dto);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, CandidateUpdateViewModel vm)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un candidato mientras exista una elección activa";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var updateDto = _mapper.Map<CandidateUpdateDto>(vm);

                var currentDto = await _service.GetByIdAsync(id);
                string? currentPhotoPath = "";

                if (currentDto != null)
                {
                    currentPhotoPath = currentDto.Photo;
                }

                if (vm.Photo != null)
                {
                    var newPhoto = FileManager.Upload(vm.Photo, id, "Candidates", true, currentPhotoPath);
                    if (string.IsNullOrEmpty(newPhoto))
                    {
                        ModelState.AddModelError("Photo", "La foto debe ser una imagen válida (.jpg, .jpeg, .png).");
                        return View(vm);
                    }
                    updateDto.Photo = newPhoto;
                }
                else
                {
                    updateDto.Photo = currentPhotoPath;  
                }

                await _service.UpdateAsync(updateDto, userId);
                TempData["SuccessMessage"] = "Candidato actualizado exitosamente.";
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
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                TempData["ErrorMessage"] = "No se pueden realizar cambios mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var candidate = await _service.GetByIdAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
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
                var candidate = await _service.GetByIdAsync(id);
                if (candidate == null)
                {
                    return NotFound();
                }

                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (candidate.IsActive)
                {
                    await _service.DeleteAsync(id, userId);
                    TempData["SuccessMessage"] = "Candidato desactivado exitosamente.";
                }
                else
                {
                    // Activar: usa UpdateAsync con IsActive = true
                    var dto = _mapper.Map<CandidateUpdateDto>(candidate);
                    dto.IsActive = true;
                    await _service.UpdateAsync(dto, userId);
                    TempData["SuccessMessage"] = "Candidato activado exitosamente.";
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