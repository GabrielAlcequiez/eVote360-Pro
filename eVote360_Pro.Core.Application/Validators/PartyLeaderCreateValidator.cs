using FluentValidation;

namespace eVote360_Pro.Core.Application.DTOs.PartyLeader
{
    public class PartyLeaderCreateValidator : AbstractValidator<PartyLeaderCreateDto>
    {
        public PartyLeaderCreateValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El ID del usuario es requerido.");

            RuleFor(x => x.PoliticalPartyId)
                .NotEmpty().WithMessage("El ID del partido político es requerido.");
        }
    }
}
