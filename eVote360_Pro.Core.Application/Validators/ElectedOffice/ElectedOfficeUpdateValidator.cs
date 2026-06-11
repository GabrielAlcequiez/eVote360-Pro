using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Validators.ElectedOffice
{
    public class ElectedOfficeUpdateValidator : AbstractValidator<ElectedOfficeUpdateDto>
    {
        private readonly IElectedOfficeRepository _repository;
        public ElectedOfficeUpdateValidator(IElectedOfficeRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del cargo electivo es requerido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del puesto electivo es requerido.")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.")
                .MustAsync(BeUniqueName)
                .WithMessage("Ya existe un puesto electivo con este nombre");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del cargo electivo es requerida.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
        }

        // helpers
        private async Task<bool>BeUniqueName(ElectedOfficeUpdateDto dto, string name, CancellationToken ct)
        {
            var existing = await _repository.GetByName(name.Trim());
            return existing == null || existing.Id == dto.Id;
        }
    }
}
