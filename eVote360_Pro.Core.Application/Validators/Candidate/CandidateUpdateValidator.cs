using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.Candidate;

namespace eVote360_Pro.Core.Application.Validators.Candidate
{
    public class CandidateUpdateValidator : AbstractValidator<CandidateUpdateDto>
    {
        public CandidateUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del candidato es requerido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del candidato es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido del candidato es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

            RuleFor(x => x.Photo)
                .NotEmpty().WithMessage("La foto del candidato es requerida.");
        }
    }
}
