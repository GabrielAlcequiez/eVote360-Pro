using FluentValidation;

namespace eVote360_Pro.Core.Application.DTOs.Citizen
{
    public class CitizenUpdateValidator : AbstractValidator<CitizenUpdateDto>
    {
        public CitizenUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID del ciudadano es requerido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es requerido.")
                .MaximumLength(20).WithMessage("El número de documento no puede superar los 20 caracteres.");
        }
    }
}
