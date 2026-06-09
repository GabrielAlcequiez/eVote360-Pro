using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;

namespace eVote360_Pro.Core.Application.Validators.CandidateOfficeAssignment
{
    public class CandidateOfficeAssignmentCreateValidator : AbstractValidator<CandidateOfficeAssignmentCreateDto>
    {
        public CandidateOfficeAssignmentCreateValidator()
        {
            RuleFor(x => x.CandidateId)
                .NotEmpty().WithMessage("El candidato es requerido.");

            RuleFor(x => x.ElectedOfficeId)
                .NotEmpty().WithMessage("El cargo electivo es requerido.");

            RuleFor(x => x.PoliticalPartyId)
                .NotEmpty().WithMessage("El partido político postulante es requerido.");
        }
    }
}
