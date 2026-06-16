using System.Runtime.CompilerServices;
using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;

namespace eVote360_Pro.Core.Application.Services
{
    public class CandidateOfficeAssignmentService : ICandidateOfficeAssignmentService
    {
        private readonly ICandidateOfficeAssignmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CandidateOfficeAssignmentCreateDto> _createValidator;
        private readonly IElectionRepository _electionRepository;
        private readonly IPartyLeaderRepository _partyLeaderRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IPoliticalAllianceRepository _politicalAllianceRepository;
        private readonly IElectedOfficeRepository _electedOfficeRepository;
        private readonly IPoliticalPartyRepository _politicalPartyRepository;

        public CandidateOfficeAssignmentService(
            ICandidateOfficeAssignmentRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IValidator<CandidateOfficeAssignmentCreateDto> createValidator,
            IElectionRepository electionRepository,
            IPartyLeaderRepository partyLeaderRepository,
            ICandidateRepository candidateRepository,
            IPoliticalAllianceRepository politicalAllianceRepository,
            IElectedOfficeRepository electedOfficeRepository,
            IPoliticalPartyRepository politicalPartyRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _electionRepository = electionRepository;
            _partyLeaderRepository = partyLeaderRepository;
            _candidateRepository = candidateRepository;
            _politicalAllianceRepository = politicalAllianceRepository;
            _electedOfficeRepository = electedOfficeRepository;
            _politicalPartyRepository = politicalPartyRepository;
        }

        public async Task<CandidateOfficeAssignmentGetDto?> AddAssignment(CandidateOfficeAssignmentCreateDto dto, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var partyLeader = await _partyLeaderRepository.GetPartyLeaderDetailsAsync(userId);

            if (partyLeader is null)
                throw new InvalidOperationException("El usuario autenticado no tiene un partido político asignado.");

            if (partyLeader.PoliticalParty is null)
                throw new InvalidOperationException("No se encontró el partido político asociado al dirigente.");

            if (!partyLeader.PoliticalParty.IsActive)
                throw new InvalidOperationException("El partido político del dirigente se encuentra inactivo.");


            var candidate = await _candidateRepository.GetByIdAsync(dto.CandidateId);
            if (candidate is null)
                throw new InvalidOperationException("El candidato no existe");
            if (!candidate.IsActive)
                throw new InvalidOperationException("El candidato se encuentra inactivo");

            var office = await _electedOfficeRepository.GetByIdAsync(dto.ElectedOfficeId);
            if (office is null)
                throw new InvalidOperationException("El puesto electivo  no existe");
            if (!office.IsActive)
                throw new InvalidOperationException("El puesto electivo se encuentra inactivo");

            if (await _repository.CandidateHasOfficeAssigned(dto.CandidateId, partyLeader.PoliticalPartyId))
                throw new InvalidOperationException("El candidato seleccionado ya está asignado a un puesto dentro del partido.");

            if (await _repository.OfficeHasCandidateAssigned(dto.ElectedOfficeId, partyLeader.PoliticalPartyId))
                throw new InvalidOperationException("El puesto seleccionado ya tiene asignado a un candidato dentro del partido.");

            // Validaciones de alianza
            if (candidate.PoliticalPartyId != partyLeader.PoliticalPartyId)
            {
                if (!await _politicalAllianceRepository.HasActiveAllianceBetweenPartiesAsync(partyLeader.PoliticalPartyId, candidate.PoliticalPartyId))
                    throw new InvalidOperationException("No existe una alianza vigente con el partido de este candidato");

                var candidateParty =
                    await _politicalPartyRepository.GetByIdAsync(candidate.PoliticalPartyId);

                if (candidateParty is null || !candidateParty.IsActive)
                    throw new InvalidOperationException(
                        "El partido de origen del candidato se encuentra inactivo.");
                var assignmentAlliance = await _repository.GetAssignmentByCandidateAndParty(candidate.Id, candidate.PoliticalPartyId);
                if (assignmentAlliance is null)
                    throw new InvalidOperationException("El candidato no tiene asignación en el partido de origen");

                if (assignmentAlliance.ElectedOfficeId != dto.ElectedOfficeId)
                    throw new InvalidOperationException("El candidato no aspira al mismo puesto en su partido de origen");

            }

            var candidateOfficeAssignment = new CandidateOfficeAssignment(
                dto.ElectedOfficeId,
                dto.CandidateId,
                partyLeader.PoliticalPartyId,
                candidate.PoliticalPartyId);

            await _repository.AddAsync(candidateOfficeAssignment);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CandidateOfficeAssignmentGetDto>(candidateOfficeAssignment);
        }

        public async Task<bool> DeleteAssignment(Guid id, Guid userId)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId);

            if (partyLeader is null)
                throw new InvalidOperationException("El usuario autenticado no tiene un partido político asignado.");

            var assignment = await _repository.GetByIdAsync(id);
            if (assignment is null)
                throw new KeyNotFoundException("La asignación no existe o ya fue eliminada");

            if (assignment.PoliticalPartyId != partyLeader.PoliticalPartyId)
                throw new InvalidOperationException("No tiene permisos para eliminar esta asignación");

            await _repository.DeletePhysicallyAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<IReadOnlyList<CandidateOfficeAssignmentGetDto>> GetAllAssignmentsAsync(Guid userId)
        {
            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId);

            if (partyLeader is null)
                throw new InvalidOperationException(
                    "El usuario autenticado no tiene un partido político asignado.");

            var assignments =
                await _repository.GetAllByPartyIdWithDetailsAsync(
                    partyLeader.PoliticalPartyId);

            return _mapper.Map<List<CandidateOfficeAssignmentGetDto>>(assignments);
        }

        public async Task<CandidateOfficeAssignmentGetDto?> GetById(Guid id, Guid userId)
        {
            var partyLeader = await _partyLeaderRepository.GetByIdAsync(userId);

            if (partyLeader is null)
                throw new InvalidOperationException(
                    "El usuario autenticado no tiene un partido político asignado.");

            var assignment = await _repository.GetByIdWithDetailsAsync(id);

            if (assignment is null)
                throw new KeyNotFoundException(
                    "La asignación seleccionada no existe.");

            if (assignment.PoliticalPartyId != partyLeader.PoliticalPartyId)
                throw new InvalidOperationException(
                    "No tiene permisos para ver esta asignación.");

            return _mapper.Map<CandidateOfficeAssignmentGetDto>(assignment);
        }
    }
}