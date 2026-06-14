using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.User;

namespace eVote360_Pro.WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserCreateDto> _createValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;

        public UserController(
            IUserService userService,
            IElectionRepository electionRepository,
            IMapper mapper,
            IValidator<UserCreateDto> createValidator,
            IValidator<UserUpdateDto> updateValidator)
        {
            _userService = userService;
            _electionRepository = electionRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            return await _electionRepository.ValidateNoActiveElectionAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            var hasActiveElection = await IsElectionActiveAsync();

            var model = new UserIndexViewModel
            {
                Users = users,
                HasActiveElection = hasActiveElection
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new UserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede crear un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Mapeo automático de ViewModel a DTO
            var dto = _mapper.Map<UserCreateDto>(model);

            // Validar usando FluentValidation
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            try
            {
                await _userService.AddAsync(dto);
                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
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
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Mapeo automático de DTO a ViewModel
            var model = _mapper.Map<UserUpdateViewModel>(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateViewModel model)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se puede editar un usuario mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Mapeo automático de ViewModel a DTO
            var dto = _mapper.Map<UserUpdateDto>(model);

            // Validar usando FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            try
            {
                await _userService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            if (await IsElectionActiveAsync())
            {
                TempData["ErrorMessage"] = "No se pueden realizar cambios en los usuarios mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                if (user.IsActive)
                {
                    await _userService.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Usuario desactivado exitosamente.";
                }
                else
                {
                    var dto = _mapper.Map<UserUpdateDto>(user);

                    dto.IsActive = true;
                    dto.Password = null;
                    await _userService.UpdateAsync(dto);
                    TempData["SuccessMessage"] = "Usuario activado exitosamente.";
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