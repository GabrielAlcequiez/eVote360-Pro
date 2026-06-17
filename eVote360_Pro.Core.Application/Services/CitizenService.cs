using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;

namespace eVote360_Pro.Core.Application.Services
{
    public class CitizenService : ICitizenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICitizenRepository _repository;
        private readonly IValidator<CitizenCreateDto> _createValidator;
        private readonly IValidator<CitizenUpdateDto> _updateValidator;
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;
        public CitizenService(
            IUnitOfWork unitOfWork,
            ICitizenRepository repository,
            IValidator<CitizenCreateDto> createValidator,
            IValidator<CitizenUpdateDto> updateValidator,
            IElectionRepository electionRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }
        public async Task<CitizenGetDto> AddAsync(CitizenCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var citizen = new Citizen(
                dto.Name,
                dto.LastName,
                dto.Email,
                dto.DocumentNumber
            );

            await _repository.AddAsync(citizen);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CitizenGetDto>(citizen);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var citizen = await _repository.GetByIdAsync(id);

            _ = await _repository.SoftDeleteAsync(id) ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IReadOnlyList<CitizenGetDto>> GetAllAsync()
        {
            var citizens = await _repository.GetAllAsync();
            return _mapper.Map<List<CitizenGetDto>>(citizens);
        }

        public async Task<CitizenGetDto?> GetByIdAsync(Guid id)
        {
            var citizen = await _repository.GetByIdAsync(id);
            return _mapper.Map<CitizenGetDto>(citizen);
        }

        public async Task UpdateAsync(CitizenUpdateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _updateValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var citizen = await _repository.GetByIdAsync(dto.Id) 
                ?? throw new KeyNotFoundException("El ciudadano no fue encontrado");

            if (await _repository.HasBeenUsedInAnyElectionAsync(dto.Id))
            {
                if(citizen.DocumentNumber != dto.DocumentNumber)
                {
                    throw new InvalidOperationException("No se puede actualizar el documento de identidad de este ciudadano porque ya participó en una elección.");
                }
                else
                {
                    citizen.Update(
                        dto.Name,
                        dto.LastName,
                        dto.Email,
                        citizen.DocumentNumber,
                        dto.IsActive
                    );
                }
            }
            else
            {
                citizen.Update(
                        dto.Name,
                        dto.LastName,
                        dto.Email,
                        dto.DocumentNumber,
                        dto.IsActive
                    );
            }
            await _repository.UpdateAsync(citizen.Id, citizen);
            await _unitOfWork.CompleteAsync();
        }
    }
}