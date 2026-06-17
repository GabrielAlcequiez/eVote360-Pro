using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Election;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using FluentValidation;


namespace eVote360_Pro.Core.Application.Services
{
    public class ElectionService : IElectionService
    {
        private readonly IElectionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ElectionCreateDto> _createValidator;
        private readonly IElectedOfficeRepository _electedOfficeRepository;
        private readonly IPoliticalPartyRepository _politicalPartyRepository;
        private readonly ICandidateOfficeAssignmentRepository _candidateOfficeAssignmentRepository;
        private readonly IMapper _mapper;

        public ElectionService(
            IElectionRepository repository,
            IUnitOfWork unitOfWork,
            IValidator<ElectionCreateDto> createValidator,
            IElectedOfficeRepository electedOfficeRepository,
            IPoliticalPartyRepository politicalPartyRepository,
            ICandidateOfficeAssignmentRepository candidateOfficeAssignmentRepository,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _electedOfficeRepository = electedOfficeRepository;
            _politicalPartyRepository = politicalPartyRepository;
            _candidateOfficeAssignmentRepository = candidateOfficeAssignmentRepository;
            _mapper = mapper;
        }

        public async Task<ElectionGetDto?> AddElection(ElectionCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _repository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var activeOffices = await _electedOfficeRepository.GetActiveElectedOffice();
            var activeParties = await _politicalPartyRepository.GetActivePoliticalPartiesAsync();
            var assignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();

            if (!activeOffices.Any())
                throw new InvalidOperationException("No hay puestos electivos activos para realizar una elección.");

            if (activeParties.Count < 2)
                throw new InvalidOperationException("No hay suficientes partidos politicos para realizar una elección.");

            var existingAssignmentsLookUp = assignments
                .Select(a => new { a.PoliticalPartyId, a.ElectedOfficeId })
                .ToHashSet();

            var partiesWithMissingOffices = activeParties
                .Select(party => new
                {
                    Party = party,
                    MissingOffices = activeOffices
                        .Where(office => !existingAssignmentsLookUp.Contains(new { PoliticalPartyId = party.Id, ElectedOfficeId = office.Id }))
                        .ToList()
                })
                .Where(x => x.MissingOffices.Any())
                .ToList();

            if (partiesWithMissingOffices.Any())
            {
                var errorMessages = new List<string>();
                foreach (var item in partiesWithMissingOffices)
                {
                    string missingOfficeNames = string.Join(", ", item.MissingOffices.Select(o => o.Name));
                    string message = $"El partido político {item.Party.Name} ({item.Party.Acronym}) no tiene candidatos activos asignados para los siguientes puestos electivos: {missingOfficeNames}.";
                    errorMessages.Add(message);
                }
                throw new InvalidOperationException(string.Join(" ", errorMessages));
            }

            var election = new Election(
                dto.Name,
                dto.ScheduledDate
            );

            await _repository.AddAsync(election);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ElectionGetDto>(election);
        }

        public async Task<bool> ActivateElection(Guid id)
        {
            var election = await _repository.GetByIdAsync(id);
            if (election is null)
                throw new InvalidOperationException("La elección no existe.");

            election.Activate();

            var activeOffices = await _electedOfficeRepository.GetActiveElectedOffice();
            var activeParties = await _politicalPartyRepository.GetActivePoliticalPartiesAsync();
            var assignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();

            if (!activeOffices.Any())
                throw new InvalidOperationException("No hay puestos electivos activos para activar esta elección.");

            if (activeParties.Count < 2)
                throw new InvalidOperationException("No hay suficientes partidos políticos para activar esta elección.");

            var existingAssignmentsLookUp = assignments
                .Select(a => new { a.PoliticalPartyId, a.ElectedOfficeId })
                .ToHashSet();

            var partiesWithMissingOffices = activeParties
                .Select(party => new
                {
                    Party = party,
                    MissingOffices = activeOffices
                        .Where(office => !existingAssignmentsLookUp.Contains(new { PoliticalPartyId = party.Id, ElectedOfficeId = office.Id }))
                        .ToList()
                })
                .Where(x => x.MissingOffices.Any())
                .ToList();

            if (partiesWithMissingOffices.Any())
            {
                var errorMessages = new List<string>();
                foreach (var item in partiesWithMissingOffices)
                {
                    string missingOfficeNames = string.Join(", ", item.MissingOffices.Select(o => o.Name));
                    string message = $"El partido político {item.Party.Name} ({item.Party.Acronym}) no tiene candidatos activos asignados para los siguientes puestos electivos: {missingOfficeNames}.";
                    errorMessages.Add(message);
                }
                throw new InvalidOperationException(string.Join(" ", errorMessages));
            }

            await _repository.UpdateAsync(id, election);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> FinishElection(Guid id)
        {
            var election = await _repository.GetByIdAsync(id);
            if (election is null)
                throw new InvalidOperationException("La elección no existe.");

            election.FinalizeElection();

            await _repository.UpdateAsync(id, election);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IReadOnlyList<ElectionListDto>> GetAllAsync()
        {
            var elections = await _repository.GetAllAsync();
            var activeOffices = await _electedOfficeRepository.GetActiveElectedOffice();
            var activeParties = await _politicalPartyRepository.GetActivePoliticalPartiesAsync();

            var list = elections
                .OrderByDescending(e => e.Status == ElectionStatus.Active ? 1 : 0)
                .ThenByDescending(e => e.ScheduledDate)
                .Select(e =>
                {
                    var dto = _mapper.Map<ElectionListDto>(e);
                    dto.PartyCount = activeParties.Count;
                    dto.OfficeCount = activeOffices.Count;
                    dto.VoterCount = 0;
                    return dto;
                })
                .ToList();

            return list.AsReadOnly();
        }

        public async Task<ElectionGetDto?> GetByIdAsync(Guid id)
        {
            var election = await _repository.GetByIdAsync(id);
            return election is null ? null : _mapper.Map<ElectionGetDto>(election);
        }
    }
}