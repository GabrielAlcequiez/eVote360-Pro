using FluentValidation;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Validators.Citizen
{
    public class CitizenUpdateValidator : AbstractValidator<CitizenUpdateDto>
    {
        private readonly ICitizenRepository _repository;
        public CitizenUpdateValidator(ICitizenRepository repository)
        {
            _repository = repository;

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
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres.")
                .MustAsync(BeUniqueEmail)
                .WithMessage("Ya hay ciudadano registrado con este email.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es requerido.")
                .Must(dn => {
                    var clean = dn.Replace("-", "").Trim();
                    return clean.Length == 11 && clean.All(char.IsDigit);
                }).WithMessage("El número de documento debe tener exactamente 11 dígitos.")
                .MustAsync(BeUniqueDocumentNumber)
                .WithMessage("Ya hay un ciudadano registrado con este número de documento");
        }

        private async Task<bool>BeUniqueEmail(CitizenUpdateDto dto, string email, CancellationToken ct)
        {
            var existing = await _repository.GetByEmail(email.Trim());
            return existing == null || existing.Id == dto.Id;
        }

        private async Task<bool>BeUniqueDocumentNumber(CitizenUpdateDto dto, string documentNumber, CancellationToken ct)
        {
            var clean = documentNumber.Replace("-", "").Trim();
            var existing = await _repository.GetByDocumentNumber(clean);
            return existing == null || existing.Id == dto.Id;
        }
    }
}
