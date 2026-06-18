using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;

namespace eVote360_Pro.Core.Application.Services
{
    public class CandidateService : ICandidateService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICandidateRepository _repository;
        private readonly IPartyLeaderRepository _partyLeaderRepository;
        private readonly IValidator<CandidateCreateDto> _createValidator;
        private readonly IValidator<CandidateUpdateDto> _updateValidator;
        private readonly IElectionRepository _electionRepository;
        private readonly ICandidateOfficeAssignmentRepository _candidateOfficeAssignmentRepository;
        private readonly IMapper _mapper;
        public CandidateService(
            IUnitOfWork unitOfWork,
            ICandidateRepository repository,
            IPartyLeaderRepository partyLeaderRepository,
            IValidator<CandidateCreateDto> createValidator,
            IValidator<CandidateUpdateDto> updateValidator,
            IElectionRepository electionRepository,
            ICandidateOfficeAssignmentRepository candidateOfficeAssignmentRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _partyLeaderRepository = partyLeaderRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _electionRepository = electionRepository;
            _candidateOfficeAssignmentRepository = candidateOfficeAssignmentRepository;
            _mapper = mapper;

        }

        public async Task<CandidateGetDto> AddAsync(CandidateCreateDto dto, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            if (!await _partyLeaderRepository.HasPartyAssignmentAsync(userId))
                throw new InvalidOperationException("No puede crear candidatos porque no tiene un partido político asignado.");

            if (!await _partyLeaderRepository.HasActivePoliticalPartyAsync(userId))
                throw new InvalidOperationException("No puede crear candidatos porque el partido político se encuentra inactivo.");
        
            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("Dirigente no encontrado");

            var candidate = new Candidate(
               dto.Name,
               dto.LastName,
               dto.Photo,
               partyLeader.PoliticalPartyId
               );

            await _repository.AddAsync(candidate);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CandidateGetDto>(candidate);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var candidate = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("El candidato no fue encontrado");

            if(!candidate.IsActive)
                 throw new InvalidOperationException("Este candidato ya se encuentra inactivo.");

            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("El dirigente político no fue encontrado");
                
            if(candidate.PoliticalPartyId != partyLeader.PoliticalPartyId)
                throw new InvalidOperationException("No tiene permisos para desactivar este candidato.");
            
            if(await _candidateOfficeAssignmentRepository.CandidateHasOfficeAssigned(candidate.Id, candidate.PoliticalPartyId))
                throw new InvalidOperationException("No se puede desactivar este candidato porque está asignado a un puesto electivo.");
                
            _ = await _repository.SoftDeleteAsync(id)
                ?? throw new KeyNotFoundException("El candidato no existe");
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IReadOnlyList<CandidateGetDto>> GetAllAsync(Guid userId)
        {
            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId);
            if(partyLeader == null)
                throw new KeyNotFoundException("No se pudo encontrar el dirigente");
            
            var candidates = await _repository.GetAllWithParty(partyLeader.PoliticalPartyId);
            return _mapper.Map<List<CandidateGetDto>>(candidates);
        }

        public async Task<CandidateGetDto?> GetByIdAsync(Guid id)
        {
            var candidate = await _repository.GetByIdAsync(id);
            return _mapper.Map<CandidateGetDto>(candidate);
        }

        public async Task UpdateAsync(CandidateUpdateDto dto, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _updateValidator.ValidateAndThrowAsync(dto);

            if(await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            if (!await _partyLeaderRepository.HasPartyAssignmentAsync(userId))
                throw new InvalidOperationException("No puede editar el candidato porque no tiene un partido político asignado.");

            if (!await _partyLeaderRepository.HasActivePoliticalPartyAsync(userId))
                throw new InvalidOperationException("No puede editar el candidato porque el partido político se encuentra inactivo.");
       
            var candidate = await _repository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException("El ciudadano no fue encontrado");

            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("El dirigente político no fue encontrado");

            if(candidate.PoliticalPartyId != partyLeader.PoliticalPartyId)
                throw new InvalidOperationException("No tiene permisos para modificar este candidato.");
            if(await _repository.HasBeenUsedInElectionAsync(dto.Id))
            {
                if(candidate.Name != dto.Name || candidate.LastName != dto.LastName || candidate.Photo != dto.Photo)
                {
                    throw new InvalidOperationException("No se pueden actualizar los datos del candidato porque ya participó en una elección.");
                }
                else
                {
                    candidate.Update(
                        candidate.Name,
                        candidate.LastName,
                        candidate.Photo,
                        dto.IsActive
                    );
                }
            }
            else
            {
                candidate.Update(
                        dto.Name,
                        dto.LastName,
                        string.IsNullOrWhiteSpace(dto.Photo) ? candidate.Photo : dto.Photo,
                        dto.IsActive
                    );
            }

            await _repository.UpdateAsync(candidate.Id, candidate);
            await _unitOfWork.CompleteAsync();
        }
    }
}