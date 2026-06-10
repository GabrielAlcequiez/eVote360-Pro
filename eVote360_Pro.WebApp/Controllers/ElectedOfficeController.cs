using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.WebApp.ViewModels.ElectedOffice;
using Microsoft.AspNetCore.Mvc;

namespace eVote360_Pro.WebApp.Controllers
{
    public class ElectedOfficeController : Controller
    {
        private readonly IElectedOfficeService _service;
        private readonly IMapper _mapper;

        public ElectedOfficeController(IElectedOfficeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var vm = _mapper.Map<IReadOnlyList<ElectedOfficeGetViewModel>>(dtos);
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ElectedOfficeCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElectedOfficeCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var createDto = _mapper.Map<ElectedOfficeCreateDto>(vm);

                await _service.AddAsync(createDto);
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
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
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var updateDto = _mapper.Map<ElectedOfficeUpdateDto>(vm);
                updateDto.Id = id;
                await _service.UpdateAsync(updateDto);
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);

                var vm = _mapper.Map<ElectedOfficeGetViewModel>(dto);
                return View(vm);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);

                TempData["DeleteMessage"] = "El puesto electivo ha sido desactivado correctamente.";
                TempData["DeleteType"] = "soft";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["DeleteMessage"] = "El puesto electivo no fue encontrado.";
                TempData["DeleteType"] = "error";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["DeleteMessage"] = ex.Message;
                TempData["DeleteType"] = "error";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}