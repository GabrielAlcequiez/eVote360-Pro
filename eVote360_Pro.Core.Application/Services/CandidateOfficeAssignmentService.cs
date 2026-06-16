using System.Runtime.CompilerServices;
using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
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

        public async Task<List<CandidateGetDto>> GetAvailableCandidatesAsync(Guid partyId)
        {
            var allCandidates = await _candidateRepository.GetAllAsync();
            var allParties = await _politicalPartyRepository.GetAllAsync();
            var partyDict = allParties.ToDictionary(p => p.Id);
            var allAssignments = await _repository.GetAllWithDetailsAsync();

            var ownPartyAssignedCandidateIds = allAssignments
                .Where(a => a.PoliticalPartyId == partyId)
                .Select(a => a.CandidateId)
                .ToHashSet();

            var alliedPartyIds = new List<Guid>();
            foreach (var party in allParties.Where(p => p.IsActive && p.Id != partyId))
            {
                if (await _politicalAllianceRepository.HasActiveAllianceBetweenPartiesAsync(partyId, party.Id))
                    alliedPartyIds.Add(party.Id);
            }

            var alliedCandidateIds = new HashSet<Guid>();
            if (alliedPartyIds.Count != 0)
            {
                var alliedAssignments = allAssignments
                    .Where(a => alliedPartyIds.Contains(a.PoliticalPartyId))
                    .GroupBy(a => a.CandidateId)
                    .ToDictionary(g => g.Key, g => g.First());

                foreach (var candidate in allCandidates.Where(c => c.IsActive && alliedPartyIds.Contains(c.PoliticalPartyId)))
                {
                    if (!ownPartyAssignedCandidateIds.Contains(candidate.Id) && alliedAssignments.ContainsKey(candidate.Id))
                        alliedCandidateIds.Add(candidate.Id);
                }
            }

            var result = new List<CandidateGetDto>();

            foreach (var candidate in allCandidates.Where(c => c.IsActive))
            {
                bool isOwn = candidate.PoliticalPartyId == partyId && !ownPartyAssignedCandidateIds.Contains(candidate.Id);
                bool isAllied = alliedCandidateIds.Contains(candidate.Id);

                if (!isOwn && !isAllied)
                    continue;

                var party = partyDict.GetValueOrDefault(candidate.PoliticalPartyId);
                result.Add(new CandidateGetDto
                {
                    Id = candidate.Id,
                    Name = candidate.Name,
                    LastName = candidate.LastName,
                    Photo = candidate.Photo,
                    IsActive = candidate.IsActive,
                    PoliticalPartyId = candidate.PoliticalPartyId,
                    PoliticalPartyName = party?.Name ?? string.Empty,
                    PoliticalPartyAcronym = party?.Acronym ?? string.Empty,
                    PoliticalPartyLogo = party?.Logo ?? string.Empty
                });
            }

            return result;
        }

        public async Task<List<ElectedOfficeGetDto>> GetAvailableOfficesAsync(Guid partyId)
        {
            var allOffices = await _electedOfficeRepository.GetAllAsync();
            var assignments = await _repository.GetAllByPartyIdWithDetailsAsync(partyId);
            var assignedOfficeIds = assignments.Select(a => a.ElectedOfficeId).ToHashSet();

            var available = allOffices
                .Where(o => o.IsActive && !assignedOfficeIds.Contains(o.Id))
                .ToList();

            return _mapper.Map<List<ElectedOfficeGetDto>>(available);
        }
    }
}