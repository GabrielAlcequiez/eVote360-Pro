using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;

namespace eVote360_Pro.Core.Application.Validators.PoliticalAlliance
{
    public class PoliticalAllianceCreateValidator : AbstractValidator<PoliticalAllianceCreateDto>
    {
        public PoliticalAllianceCreateValidator()
        {
            RuleFor(x => x.ReceiverPartyId)
                .NotEmpty().WithMessage("El partido receptor de la alianza es requerido.");
        }
    }
}
