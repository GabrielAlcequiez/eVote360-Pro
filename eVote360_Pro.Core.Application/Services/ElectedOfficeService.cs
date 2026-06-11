using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Application.Interfaces;
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
        private readonly IElectionRepository _electionRepository;
        private readonly IMapper _mapper;
        public ElectedOfficeService(
            IUnitOfWork unitOfWork,
            IElectedOfficeRepository repository,
            IValidator<ElectedOfficeCreateDto> createValidator,
            IValidator<ElectedOfficeUpdateDto> updateValidator,
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

        public async Task<ElectedOfficeGetDto> AddAsync(ElectedOfficeCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var existingByName = await _repository.GetByName(dto.Name);
            if (existingByName != null)
                throw new InvalidOperationException("Ya existe un puesto electivo registrado con este nombre");

            var newElectedOffice = new ElectedOffice(
                dto.Name,
                dto.Description
            );

            await _repository.AddAsync(newElectedOffice);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ElectedOfficeGetDto>(newElectedOffice);
            
        }

        public async Task DeleteAsync(Guid id)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var entity = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Puesto electoral no encontrado");

            if (await _repository.HasActiveCandidatesAssignedAsync(id))
                throw new InvalidOperationException("No se puede desactivar este puesto electivo porque tiene candidatos activos asignados.");

            await _repository.SoftDeleteAsync(id);
            await _unitOfWork.CompleteAsync();

        }

        public async Task<IReadOnlyList<ElectedOfficeGetDto>> GetAllAsync()
        {
            var electedOffices = await _repository.GetAllAsync();
            return _mapper.Map<List<ElectedOfficeGetDto>>(electedOffices);
        }

        public async Task<ElectedOfficeGetDto?> GetByIdAsync(Guid id)
        {
            var electedOffice = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("No existe un puesto electivo con este id");
            
            return _mapper.Map<ElectedOfficeGetDto>(electedOffice);
        }

        public async Task UpdateAsync(ElectedOfficeUpdateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _updateValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");
            
            var existingByName = await _repository.GetByName(dto.Name);
            if (existingByName != null && existingByName.Id != dto.Id)
                throw new InvalidOperationException("Ya existe un puesto electivo registrado con este nombre");

            var electedOffice = await _repository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("No existe un puesto electivo con este id");

            if (await _repository.HasBeenUsedInElectionAsync(dto.Id))
            {
                if (electedOffice.Name != dto.Name)
                {
                    throw new InvalidOperationException("No se puede modificar el nombre de este puesto electivo porque ya fue utilizado en una elección.");
                }
            }

            electedOffice.Update(
                dto.Name,
                dto.Description,
                dto.IsActive
            );

            await _repository.UpdateAsync(dto.Id, electedOffice);
            await _unitOfWork.CompleteAsync();
        }
    }
}