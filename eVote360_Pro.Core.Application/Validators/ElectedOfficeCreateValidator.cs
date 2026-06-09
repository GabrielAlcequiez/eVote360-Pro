using FluentValidation;

namespace eVote360_Pro.Core.Application.DTOs.ElectedOffice
{
    public class ElectedOfficeCreateValidator : AbstractValidator<ElectedOfficeCreateDto>
    {
        public ElectedOfficeCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del cargo electivo es requerido.")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del cargo electivo es requerida.")
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
        }
    }
}
