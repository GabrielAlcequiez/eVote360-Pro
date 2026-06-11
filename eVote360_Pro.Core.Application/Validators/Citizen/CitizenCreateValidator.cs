using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Validators.Citizen
{
    public class CitizenCreateValidator : AbstractValidator<CitizenCreateDto>
    {
        private readonly ICitizenRepository _repository;

        public CitizenCreateValidator(ICitizenRepository repository)
        {
            _repository = repository;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres.")
                .MustAsync(BeUniqueEmail)
                .WithMessage("Ya hay un ciudadano registrado con este correo electronico.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es requerido.")
                .MaximumLength(20).WithMessage("El número de documento no puede superar los 20 caracteres.")
                .MustAsync(BeUniqueDocumentNumber)
                .WithMessage("Ya hay un ciudadano registrado con este número de documento.");
        }

        // helpers
        private async Task<bool>BeUniqueDocumentNumber(string documentNumber, CancellationToken ct)
        {
            var existing = await _repository.GetByDocumentNumber(documentNumber.Trim());
            return existing == null;
        }

        private async Task<bool>BeUniqueEmail(string email, CancellationToken ct)
        {
            var existing = await _repository.GetByEmail(email.Trim());
            return existing == null;
        }
    }
}
