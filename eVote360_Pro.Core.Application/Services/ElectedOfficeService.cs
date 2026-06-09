using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Application.Validators.ElectedOffice;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;

namespace eVote360_Pro.Core.Application.Services
{
    public class ElectedOfficeService : IElectedOfficeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElectedOfficeRepository _repository;
        private readonly IValidator<ElectedOfficeCreateDto> _createValidator;
        private readonly IValidator<ElectedOfficeUpdateDto> _updateValidator;
        public ElectedOfficeService(
            IUnitOfWork unitOfWork,
            IElectedOfficeRepository repository,
            IValidator<ElectedOfficeCreateDto> createValidator,
            IValidator<ElectedOfficeUpdateDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<ElectedOfficeGetDto> AddAsync(ElectedOfficeCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ElectedOfficeGetDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ElectedOfficeGetDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ElectedOfficeUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}