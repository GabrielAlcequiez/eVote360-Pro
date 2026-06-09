using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.Election;

namespace eVote360_Pro.Core.Application.Validators.Election
{
    public class ElectionCreateValidator : AbstractValidator<ElectionCreateDto>
    {
        public ElectionCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la elección es requerido.")
                .MaximumLength(200).WithMessage("El nombre no puede superar los 200 caracteres.");

            RuleFor(x => x.ScheduledDate)
                .NotEmpty().WithMessage("La fecha programada es requerida.");
        }
    }
}
