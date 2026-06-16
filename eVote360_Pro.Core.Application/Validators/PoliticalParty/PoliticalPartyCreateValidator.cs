using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;

namespace eVote360_Pro.Core.Application.Validators.PoliticalParty
{
    public class PoliticalPartyCreateValidator : AbstractValidator<PoliticalPartyCreateDto>
    {
        public PoliticalPartyCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del partido es requerido.")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            RuleFor(x => x.Acronym)
                .NotEmpty().WithMessage("El acrónimo es requerido.")
                .MaximumLength(10).WithMessage("El acrónimo no puede superar los 10 caracteres.");

            // desactivada porque ya de esto se encarga vm
            // RuleFor(x => x.Logo)
            //     .NotEmpty().WithMessage("El logo del partido es requerido.");
        }
    }
}
