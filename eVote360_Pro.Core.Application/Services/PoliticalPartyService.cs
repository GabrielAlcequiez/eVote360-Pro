using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;

namespace eVote360_Pro.Core.Application.Services
{
    public class PoliticalPartyService : IPoliticalPartyService
    {
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IBaseRepository<Candidate> _candidateRepository;
        private readonly IBaseRepository<Vote> _voteRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PoliticalPartyCreateDto> _createValidator;
        private readonly IValidator<PoliticalPartyUpdateDto> _updateValidator;

        public PoliticalPartyService(
            IPoliticalPartyRepository partyRepository, 
            IBaseRepository<Candidate> candidateRepository, 
            IBaseRepository<Vote> voteRepository, 
            IElectionRepository electionRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<PoliticalPartyCreateDto> createValidator, 
            IValidator<PoliticalPartyUpdateDto> updateValidator)
        {
            _partyRepository = partyRepository;
            _candidateRepository = candidateRepository;
            _voteRepository = voteRepository;
            _electionRepository = electionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PoliticalPartyGetDto> AddAsync(PoliticalPartyCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            await ValidateNoActiveElection("crear");

            var cleanAcronym = dto.Acronym.Trim().ToUpperInvariant();
            var existingByAcronym = await _partyRepository.GetByAcronymAsync(cleanAcronym);

            if (existingByAcronym != null) 
            {
                throw new InvalidOperationException("Ya existe un partido político registrado con estas siglas.");
            }

            var entity = new PoliticalParty(dto.Name, dto.Description, cleanAcronym, dto.Logo);
            await _partyRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PoliticalPartyGetDto>(entity);
        }

        public async Task UpdateAsync(PoliticalPartyUpdateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _updateValidator.ValidateAndThrowAsync(dto);

            await ValidateNoActiveElection("editar");

            var existingParty = await _partyRepository.GetByIdAsync(dto.Id) 
                ?? throw new KeyNotFoundException("El partido político no existe.");

            var cleanAcronym = dto.Acronym.Trim().ToUpperInvariant();
            if (existingParty.Acronym != cleanAcronym)
            {
                var existingByAcronym = await _partyRepository.GetByAcronymAsync(cleanAcronym);
                if (existingByAcronym != null && existingByAcronym.Id != dto.Id)
                {
                    throw new InvalidOperationException("Ya existe un partido político registrado con estas siglas.");
                }
            }

            // Validar desactivación: Solo si pasa de Activo a Inactivo
            if (existingParty.IsActive && !dto.IsActive)
            {
                await ValidateDeactivation(dto.Id);
            }

            // Validar bloqueos si ya participó en elecciones
            var hasParticipated = await _partyRepository.HasParticipatedInElectionAsync(dto.Id);
            if (hasParticipated)
            {
                if (existingParty.Name != dto.Name.Trim())
                {
                    throw new InvalidOperationException("No se puede modificar el nombre de este partido político porque ya participó en una elección.");
                }
                if (existingParty.Acronym != cleanAcronym)
                {
                    throw new InvalidOperationException("No se pueden modificar las siglas de este partido político porque ya participó en una elección.");
                }
                if (dto.Logo != null && existingParty.Logo != dto.Logo.Trim())
                {
                    throw new InvalidOperationException("No se puede modificar el logo de este partido político porque ya participó en una elección.");
                }

                var finalLogo = string.IsNullOrEmpty(dto.Logo) ? existingParty.Logo : dto.Logo;
                existingParty.UpdateSafeFields(dto.Description, finalLogo, dto.IsActive);
            }
            else
            {
                var finalLogo = string.IsNullOrEmpty(dto.Logo) ? existingParty.Logo : dto.Logo;
                existingParty.UpdateAll(dto.Name, dto.Description, cleanAcronym, finalLogo, dto.IsActive);
            }

            await _partyRepository.UpdateAsync(dto.Id, existingParty);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await ValidateNoActiveElection("eliminar/desactivar");

            _ = await _partyRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException("El partido político no existe.");

            // Como borrar lógicamente desactiva el partido, validamos candidatos y dirigentes activos
            await ValidateDeactivation(id);

            await _partyRepository.SoftDeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PoliticalPartyGetDto>> GetAllAsync()
        {
            var list = await _partyRepository.GetAllWithPartyLeadersAsync();
            return _mapper.Map<List<PoliticalPartyGetDto>>(list);
        }

        public async Task<PoliticalPartyGetDto?> GetByIdAsync(Guid id)
        {
            var entity = await _partyRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<PoliticalPartyGetDto>(entity);
        }

        public async Task<bool> HasActiveCandidatesAsync(Guid id)
        {
            return await _partyRepository.HasActiveCandidatesAsync(id);
        }

        public async Task<bool> HasParticipatedInElectionAsync(Guid id)
        {
            return await _partyRepository.HasParticipatedInElectionAsync(id);
        }

        private async Task ValidateNoActiveElection(string action)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                throw new InvalidOperationException($"No se puede {action} un partido político mientras exista una elección activa.");
            }
        }

        private async Task ValidateDeactivation(Guid id)
        {
            if (await _partyRepository.HasActiveCandidatesAsync(id))
            {
                throw new InvalidOperationException("No se puede desactivar este partido político porque tiene candidatos activos registrados.");
            }
            if (await _partyRepository.HasActiveLeaderAsync(id))
            {
                throw new InvalidOperationException("No se puede desactivar este partido político porque tiene un dirigente político asignado.");
            }
        }
    }
}