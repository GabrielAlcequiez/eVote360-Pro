using FluentValidation;

namespace eVote360_Pro.Core.Application.DTOs.Election
{
    public class ElectionUpdateValidator : AbstractValidator<ElectionUpdateDto>
    {
        public ElectionUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID de la elección es requerido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la elección es requerido.")
                .MaximumLength(200).WithMessage("El nombre no puede superar los 200 caracteres.");

            RuleFor(x => x.ScheduledDate)
                .NotEmpty().WithMessage("La fecha programada es requerida.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado de la elección no es válido.");
        }
    }
}
