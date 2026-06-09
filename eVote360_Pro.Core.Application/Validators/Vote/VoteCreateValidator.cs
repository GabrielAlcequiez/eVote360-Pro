using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.Vote;

namespace eVote360_Pro.Core.Application.Validators.Vote
{
    public class VoteCreateValidator : AbstractValidator<VoteCreateDto>
    {
        public VoteCreateValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty().WithMessage("El ID de la elección es requerido.");

            RuleFor(x => x.ElectedOfficeId)
                .NotEmpty().WithMessage("El ID del cargo electivo es requerido.");

            RuleFor(x => x.CandidateId)
                .NotEqual(Guid.Empty).WithMessage("El ID del candidato no es válido si se proporciona.")
                .When(x => x.CandidateId.HasValue);
        }
    }
}
