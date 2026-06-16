using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class PoliticalAllianceService(IPoliticalAllianceRepository politicalAllianceRepository, IPoliticalPartyRepository politicalPartyRepository, IElectionRepository electionRepository, IUnitOfWork unitOfWork, IMapper mapper) : IPoliticalAllianceService
    {
        private readonly IPoliticalAllianceRepository _politicalAllianceRepository = politicalAllianceRepository;
        private readonly IPoliticalPartyRepository _politicalPartyRepository = politicalPartyRepository;
        private readonly IElectionRepository _electionRepository = electionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AcceptRequestAsync(Guid id, Guid currentPartyId)
        {
            await ValidateNoActiveElectionAsync("aceptar una solicitud  de alianza");
            var alliance = await _politicalAllianceRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("La solicitud de alianza seleccionada no existe o ya fue eliminada.");

            if (alliance.Status != AllianceStatus.Pending)
                throw new InvalidOperationException("Esta solicitud de alianza ya fue respondida.");

            var requester = await _politicalPartyRepository.GetByIdAsync(alliance.RequesterPartyId);
            var receiver = await _politicalPartyRepository.GetByIdAsync(alliance.ReceiverPartyId);

            if (requester == null || !requester.IsActive || receiver == null || !receiver.IsActive)
                throw new InvalidOperationException("Ambos partidos políticos deben estar activos para formalizar la alianza.");

            if (await _politicalAllianceRepository.HasActiveAllianceBetweenPartiesAsync(alliance.RequesterPartyId, alliance.ReceiverPartyId))
                throw new InvalidOperationException("Ya existe una alianza vigente con este partido político.");

            alliance.Accept();
            await _politicalAllianceRepository.UpdateAsync(id, alliance);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CreateRequestAsync(Guid requesterPartyId, PoliticalAllianceCreateDto dto)
        {
            await ValidateNoActiveElectionAsync("crear una solicitud de alianza");
            if (requesterPartyId == dto.ReceiverPartyId)
                throw new InvalidOperationException("No puede crear una solicitud de alianza hacia su propio partido político.");

            var receiver = await _politicalPartyRepository.GetByIdAsync(dto.ReceiverPartyId);
            if (receiver == null || !receiver.IsActive)
                throw new InvalidOperationException("No puede crear una solicitud de alianza con un partido político inactivo o inexistente.");

            var requester = await _politicalPartyRepository.GetByIdAsync(requesterPartyId);
            if (requester == null || !requester.IsActive)
                throw new InvalidOperationException("El partido del dirigente autenticado debe estar activo.");

            if (await _politicalAllianceRepository.HasActiveAllianceBetweenPartiesAsync(requesterPartyId, dto.ReceiverPartyId))
                throw new InvalidOperationException("Ya existe una alianza vigente con este partido político.");

            var alliances = await _politicalAllianceRepository.GetAllByPartyIdWithDetailsAsync(requesterPartyId);

            bool sentPending = alliances.Any(a => a.RequesterPartyId == requesterPartyId && a.ReceiverPartyId == dto.ReceiverPartyId && a.Status == AllianceStatus.Pending);
            if (sentPending)
                throw new InvalidOperationException("Ya existe una solicitud de alianza pendiente enviada a este partido político.");

            bool receivedPending = alliances.Any(a => a.RequesterPartyId == dto.ReceiverPartyId && a.ReceiverPartyId == requesterPartyId && a.Status == AllianceStatus.Pending);

            if (receivedPending)
                throw new InvalidOperationException("Ya existe una solicitud de alianza pendiente enviada por este partido político.");
            var alliance = new PoliticalAlliance(requesterPartyId, dto.ReceiverPartyId);

            await _politicalAllianceRepository.AddAsync(alliance);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAllianceAsync(Guid id, Guid currentPartyId)
        {
            await ValidateNoActiveElectionAsync("eliminar una alianza politica");
            var alliance = await _politicalAllianceRepository.GetByIdAsync(id);

            if (alliance == null || alliance.Status != AllianceStatus.Accepted)
                throw new KeyNotFoundException("La alianza política seleccionada no existe o ya fue eliminada.");

            if (alliance.RequesterPartyId != currentPartyId && alliance.ReceiverPartyId != currentPartyId)
                throw new UnauthorizedAccessException("No tiene permisos para eliminar esta alianza política.");

            // Validación crucial: revisar si hay candidatos asignados cruzados activos entre ambos partidos
            if (await _politicalAllianceRepository.HasActiveCandidatesAssignedBetweenPartiesAsync(alliance.RequesterPartyId, alliance.ReceiverPartyId))
            {
                throw new InvalidOperationException("No se puede eliminar esta alianza porque existen candidatos aliados asignados entre estos partidos. Primero deben eliminarse las asignaciones correspondientes desde el módulo Asignar candidato a puesto.");
            }
            await _politicalAllianceRepository.DeletePhysicallyAsync(id);
            await _unitOfWork.CompleteAsync();

        }

        public async Task DeleteRequestAsync(Guid id, Guid currentPartyId)
        {
            await ValidateNoActiveElectionAsync("eliminar una  solicitud de alianza");

            var alliance = await _politicalAllianceRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("La solicitud de alianza seleccionada no existe o ya fue eliminada.");

            if (alliance.RequesterPartyId != currentPartyId)
                throw new UnauthorizedAccessException("No tiene permisos para eliminar esta solicitud de alianza.");

            if (alliance.Status == AllianceStatus.Accepted)
                throw new InvalidOperationException("No se puede eliminar una solicitud aceptada porque ya generó una alianza vigente. Para terminarla debe eliminar la alianza desde el listado de alianzas vigentes.");

            await _politicalAllianceRepository.DeletePhysicallyAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PoliticalAllianceGetDto>> GetActiveAllianceAsync(Guid partyId)
        {
            var alliances = await _politicalAllianceRepository.GetAllByPartyIdWithDetailsAsync(partyId);
            var active = alliances.Where(a => a.Status == AllianceStatus.Accepted).ToList();

            return _mapper.Map<List<PoliticalAllianceGetDto>>(active);
        }

        public async Task<List<PoliticalPartyGetDto>> GetAvailablePartiesForAllAllianceAsync(Guid currentPartyId)
        {
            var allParties = await _politicalPartyRepository.GetAllAsync();
            var availableParties = new List<PoliticalParty>();

            foreach (var party in allParties.Where(p => p.IsActive && p.Id != currentPartyId))
            {
                bool hasAlliance = await _politicalAllianceRepository.HasActiveAllianceBetweenPartiesAsync(currentPartyId, party.Id);
                bool hasPending = await _politicalAllianceRepository.HasPendingRequestBetweenPartiesAsync(currentPartyId, party.Id);

                if (!hasAlliance && !hasPending)
                {
                    availableParties.Add(party);
                }
            }

            return _mapper.Map<List<PoliticalPartyGetDto>>(availableParties);
        }

        public async Task<PoliticalAllianceGetDto?> GetByIdAsync(Guid id)
        {
            var alliance = await _politicalAllianceRepository.GetByIdAsync(id);
            if (alliance == null) return null;

            var requester = await _politicalPartyRepository.GetByIdAsync(alliance.RequesterPartyId);
            var receiver = await _politicalPartyRepository.GetByIdAsync(alliance.ReceiverPartyId);

            var dto = _mapper.Map<PoliticalAllianceGetDto>(alliance);
            if (requester != null)
            {
                dto.RequesterPartyName = requester.Name;
                dto.RequesterPartyAcronym = requester.Acronym;
            }
            if (receiver != null)
            {
                dto.ReceiverPartyName = receiver.Name;
                dto.ReceiverPartyAcronym = receiver.Acronym;
            }
            return dto;

        }

        public async Task<List<PoliticalAllianceGetDto>> GetPendingRequestAsync(Guid partyId)
        {
            var alliances = await _politicalAllianceRepository.GetAllByPartyIdWithDetailsAsync(partyId);
            var pending = alliances.Where(a => a.ReceiverPartyId == partyId && a.Status == AllianceStatus.Pending).ToList();

            return _mapper.Map<List<PoliticalAllianceGetDto>>(pending);
        }

        public async Task<List<PoliticalAllianceGetDto>> GetSentRequestAsync(Guid partyId)
        {
            var alliances = await _politicalAllianceRepository.GetAllByPartyIdWithDetailsAsync(partyId);
            var sent = alliances.Where(a => a.RequesterPartyId == partyId).ToList();
            return _mapper.Map<List<PoliticalAllianceGetDto>>(sent);
        }

        public async Task RejectRequestAsync(Guid id, Guid currentPartyId)
        {
            await ValidateNoActiveElectionAsync("rechazar una solicitud de alianza");
            var alliance = await _politicalAllianceRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("La solicitud de alianza seleccionada no existe o ya fue eliminada.");

            if (alliance.ReceiverPartyId != currentPartyId)
                throw new UnauthorizedAccessException("No tiene permisos para responder esta solicitud de alianza.");

            if (alliance.Status != AllianceStatus.Pending)
                throw new InvalidOperationException("Esta solicitud de alianza ya fue respondida.");

            alliance.Reject();
            await _politicalAllianceRepository.UpdateAsync(id, alliance);
            await _unitOfWork.CompleteAsync();
        }

        private async Task ValidateNoActiveElectionAsync(string action)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                throw new InvalidOperationException($"No se puede {action} mientras exista una elección activa.");
            }
        }
    }
}