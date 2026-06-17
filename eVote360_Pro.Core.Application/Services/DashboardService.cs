using System;
using System.Linq;
using System.Threading.Tasks;
using eVote360_Pro.Core.Application.DTOs.Dashboard;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;

namespace eVote360_Pro.Core.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ICitizenRepository _citizenRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly IPoliticalPartyRepository _politicalPartyRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IElectedOfficeRepository _electedOfficeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<CitizenParticipation> _participationRepository;
        private readonly IElectionService _electionService;
        private readonly ICandidateOfficeAssignmentRepository _candidateOfficeAssignmentRepository;
        private readonly IPartyLeaderRepository _partyLeaderRepository;
        private readonly IPoliticalAllianceRepository _politicalAllianceRepository;
 
        public DashboardService(
            ICitizenRepository citizenRepository,
            IElectionRepository electionRepository,
            IPoliticalPartyRepository politicalPartyRepository,
            ICandidateRepository candidateRepository,
            IElectedOfficeRepository electedOfficeRepository,
            IUserRepository userRepository,
            IBaseRepository<CitizenParticipation> participationRepository,
            IElectionService electionService,
            ICandidateOfficeAssignmentRepository candidateOfficeAssignmentRepository,
            IPartyLeaderRepository partyLeaderRepository,
            IPoliticalAllianceRepository politicalAllianceRepository)
        {
            _citizenRepository = citizenRepository;
            _electionRepository = electionRepository;
            _politicalPartyRepository = politicalPartyRepository;
            _candidateRepository = candidateRepository;
            _electedOfficeRepository = electedOfficeRepository;
            _userRepository = userRepository;
            _participationRepository = participationRepository;
            _electionService = electionService;
            _candidateOfficeAssignmentRepository = candidateOfficeAssignmentRepository;
            _partyLeaderRepository = partyLeaderRepository;
            _politicalAllianceRepository = politicalAllianceRepository;
        }

        public async Task<DashboardGetDto> GetDashboardDataAsync(int? year = null)
        {
            // 1. Obtener contadores básicos de Ciudadanos
            var citizens = await _citizenRepository.GetAllAsync();
            var totalCitizens = citizens.Count;
            var activeCitizens = citizens.Count(c => c.IsActive);
            var inactiveCitizens = totalCitizens - activeCitizens;

            // 2. Obtener contadores básicos de Partidos y Candidatos
            var parties = await _politicalPartyRepository.GetAllAsync();
            var totalParties = parties.Count;

            var candidates = await _candidateRepository.GetAllAsync();
            var totalCandidates = candidates.Count;

            var offices = await _electedOfficeRepository.GetAllAsync();
            var totalOffices = offices.Count;

            // 3. Obtener contadores básicos de Usuarios
            var users = await _userRepository.GetAllAsync();
            var totalUsers = users.Count;
            var activeUsers = users.Count(u => u.IsActive);

            // 4. Obtener Elección Activa o Última Finalizada
            var elections = await _electionRepository.GetAllAsync();
            
            // Buscar la elección activa
            var activeElection = elections.FirstOrDefault(e => e.Status == ElectionStatus.Active);
            
            // Si no hay elección activa, buscar la última finalizada para mostrar resultados históricos
            var latestFinalizedElection = elections
                .Where(e => e.Status == ElectionStatus.Finalized)
                .OrderByDescending(e => e.ScheduledDate)
                .FirstOrDefault();

            var dto = new DashboardGetDto
            {
                TotalCitizens = totalCitizens,
                ActiveCitizens = activeCitizens,
                InactiveCitizens = inactiveCitizens,
                TotalParties = totalParties,
                TotalCandidates = totalCandidates,
                TotalElectedOffices = totalOffices,
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers
            };

            Election? targetElectionForResults = null;

            if (activeElection != null)
            {
                dto.ActiveElectionId = activeElection.Id;
                dto.ActiveElectionName = activeElection.Name;
                dto.ActiveElectionDate = activeElection.ScheduledDate;
                targetElectionForResults = activeElection;
            }
            else if (latestFinalizedElection != null)
            {
                targetElectionForResults = latestFinalizedElection;
            }

            // 5. Cargar participación y resultados si hay una elección disponible
            var participations = await _participationRepository.GetAllAsync();
            if (targetElectionForResults != null)
            {
                var votedCount = participations.Count(p => p.ElectionId == targetElectionForResults.Id);

                dto.ResultsElectionName = targetElectionForResults.Name;
                dto.ResultsElectionStatus = targetElectionForResults.Status == ElectionStatus.Active ? "Activa" : "Finalizada";
                dto.VotedCitizensCount = votedCount;
                dto.VotingParticipationPercentage = totalCitizens > 0
                    ? Math.Round((decimal)votedCount / totalCitizens * 100, 1)
                    : 0;

                // Cargar resultados agregados utilizando el servicio de elecciones existente
                var results = await _electionService.GetElectionResultsAsync(targetElectionForResults.Id);
                dto.ElectionResults = results ?? new();
            }

            // 6. Resumen electoral por año
            var availableYears = elections
                .Select(e => e.ScheduledDate.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            dto.AvailableYears = availableYears;
            int? selectedYear = year ?? availableYears.FirstOrDefault();
            dto.SelectedYear = selectedYear;

            if (selectedYear.HasValue)
            {
                var assignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();
                var participatingPartiesCount = assignments.Select(a => a.PoliticalPartyId).Distinct().Count();
                var participatingCandidatesCount = assignments.Select(a => a.CandidateId).Distinct().Count();

                var electionsInYear = elections.Where(e => e.ScheduledDate.Year == selectedYear.Value).ToList();
                foreach (var election in electionsInYear)
                {
                    dto.ElectionsForSelectedYear.Add(new ElectionSummaryDto
                    {
                        Id = election.Id,
                        Name = election.Name,
                        ScheduledDate = election.ScheduledDate,
                        ParticipatingPartiesCount = participatingPartiesCount,
                        ParticipatingCandidatesCount = participatingCandidatesCount,
                        VotedCitizensCount = participations.Count(p => p.ElectionId == election.Id)
                    });
                }
            }

            return dto;
        }

        public async Task<LeaderDashboardGetDto> GetLeaderDashboardDataAsync(Guid userId)
        {
            var leader = await _partyLeaderRepository.GetPartyLeaderDetailsAsync(userId);
            if (leader == null || leader.PoliticalParty == null)
            {
                throw new InvalidOperationException("No tiene un partido político asignado.");
            }

            var partyId = leader.PoliticalPartyId;

            // 1. Cantidad de candidatos activos e inactivos del partido
            var candidates = await _candidateRepository.GetAllAsync();
            var activeCandidatesCount = candidates.Count(c => c.PoliticalPartyId == partyId && c.IsActive);
            var inactiveCandidatesCount = candidates.Count(c => c.PoliticalPartyId == partyId && !c.IsActive);

            // 2. Alianzas políticas aprobadas (estado Aceptada)
            var alliances = await _politicalAllianceRepository.GetAllByPartyIdWithDetailsAsync(partyId);
            var alliancesCount = alliances.Count(a => a.Status == AllianceStatus.Accepted);

            // 3. Solicitudes de alianza pendientes dirigidas al partido del dirigente (receiver)
            var pendingAlliancesCount = alliances.Count(a => a.Status == AllianceStatus.Pending && a.ReceiverPartyId == partyId);

            // 4. Cantidad de candidatos asignados a puestos electivos
            var assignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();
            var assignedCandidatesCount = assignments.Count(a => a.Candidate.PoliticalPartyId == partyId);

            return new LeaderDashboardGetDto
            {
                PartyName = leader.PoliticalParty.Name,
                PartyAcronym = leader.PoliticalParty.Acronym,
                PartyLogo = leader.PoliticalParty.Logo ?? string.Empty,
                ActiveCandidatesCount = activeCandidatesCount,
                InactiveCandidatesCount = inactiveCandidatesCount,
                AlliancesCount = alliancesCount,
                PendingAlliancesCount = pendingAlliancesCount,
                AssignedCandidatesCount = assignedCandidatesCount
            };
        }
    }
}
