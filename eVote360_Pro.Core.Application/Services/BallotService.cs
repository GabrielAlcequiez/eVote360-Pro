using eVote360_Pro.Core.Application.DTOs.Ballot;
using eVote360_Pro.Core.Application.DTOs.Shared;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class BallotService : IBallotService
    {
        private readonly IElectedOfficeRepository _electedOfficeRepository;
        private readonly ICandidateOfficeAssignmentRepository _candidateOfficeAssignmentRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ICitizenRepository _citizenRepository;
        private readonly IElectionRepository _electionRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public BallotService(
            IElectedOfficeRepository electedOfficeRepository,
            ICandidateOfficeAssignmentRepository candidateOfficeAssignmentRepository,
            IVoteRepository voteRepository,
            ICitizenRepository citizenRepository,
            IElectionRepository electionRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _electedOfficeRepository = electedOfficeRepository;
            _candidateOfficeAssignmentRepository = candidateOfficeAssignmentRepository;
            _voteRepository = voteRepository;
            _citizenRepository = citizenRepository;
            _electionRepository = electionRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OfficeWithCandidatesDto>> GetOfficesWithCandidatesAsync(Guid electionId)
        {
            var assignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();

            var grouped = assignments
                .GroupBy(x => new { x.ElectedOfficeId, x.ElectedOffice.Name })
                .OrderBy(x => x.Key.Name)
                .ToList();

            var result = new List<OfficeWithCandidatesDto>();

            foreach (var group in grouped)
            {
                var candidates = group
                    .Select(x => new CandidateOptionDto
                    {
                        CandidateId = x.Candidate.Id,
                        FullName = $"{x.Candidate.Name} {x.Candidate.LastName}",
                        Photo = x.Candidate.Photo,
                        PartyName = x.PoliticalParty.Name,
                        PartyAcronym = x.PoliticalParty.Acronym,
                        PartyLogo = x.PoliticalParty.Logo
                    })
                    .ToList();

                var officeDto = new OfficeWithCandidatesDto
                {
                    OfficeId = group.Key.ElectedOfficeId,
                    OfficeName = group.Key.Name,
                    PartyCount = candidates.Select(x => x.PartyAcronym).Distinct().Count(),
                    CandidateCount = candidates.Select(x => x.CandidateId).Distinct().Count(),
                    Candidates = candidates
                };

                result.Add(officeDto);
            }

            return result;
        }

        public async Task FinalizeVotingAsync(Guid citizenId, Guid electionId, Dictionary<Guid, Guid?> selections, string citizenName, string citizenEmail, string electionName, DateTime electionDate)
        {
            var citizen = await _citizenRepository.GetByIdAsync(citizenId);
            if (citizen is null)
                throw new InvalidOperationException("El ciudadano no existe.");

            if (!citizen.IsActive)
                throw new InvalidOperationException("El ciudadano se encuentra inactivo.");

            var election = await _electionRepository.GetByIdAsync(electionId);
            if (election is null)
                throw new InvalidOperationException("La elección no existe.");

            if (election.Status != Core.Domain.Common.Enums.ElectionStatus.Active)
                throw new InvalidOperationException("La elección no está activa.");

            var alreadyVoted = await _citizenRepository.HasBeenUsedInElectionAsync(citizenId, electionId);
            if (alreadyVoted)
                throw new InvalidOperationException("El ciudadano ya ha ejercido su voto en esta elección.");

            var votes = new List<Vote>();
            foreach (var (officeId, candidateId) in selections)
            {
                votes.Add(new Vote(electionId, officeId, candidateId));
            }

            await _voteRepository.AddRangeAsync(votes);

            var participation = new CitizenParticipation(electionId, citizenId);
            await _citizenRepository.AddParticipationAsync(participation);

            await _unitOfWork.CompleteAsync();

            var officeIds = selections.Keys.ToList();
            var allAssignments = await _candidateOfficeAssignmentRepository.GetActiveCandidateOfficeAssignmentsAsync();
            var relevantAssignments = allAssignments.Where(a => officeIds.Contains(a.ElectedOfficeId)).ToList();

            var summaryItems = new List<(string OfficeName, string CandidateName, string PartyName)>();
            foreach (var (officeId, candidateId) in selections)
            {
                var officeName = relevantAssignments.FirstOrDefault(a => a.ElectedOfficeId == officeId)?.ElectedOffice.Name ?? "Desconocido";

                string candidateName, partyName;
                if (candidateId.HasValue)
                {
                    var assignment = relevantAssignments.FirstOrDefault(a => a.ElectedOfficeId == officeId && a.CandidateId == candidateId.Value);
                    candidateName = assignment is not null ? $"{assignment.Candidate.Name} {assignment.Candidate.LastName}" : $"ID: {candidateId}";
                    partyName = assignment?.PoliticalParty?.Name ?? "";
                }
                else
                {
                    candidateName = "Ninguno";
                    partyName = "";
                }

                summaryItems.Add((officeName, candidateName, partyName));
            }

            var summaryBody = BuildSummaryEmail(citizenName, electionName, electionDate, summaryItems);
            var emailRequest = new EmailRequest
            {
                ToEmail = citizenEmail,
                RecipientName = citizenName,
                Subject = "Resumen de su participación electoral",
                Body = summaryBody
            };

            try
            {
                await _emailService.SendEmailAsync(emailRequest);
            }
            catch
            {
                // Por si falla el servicio de correo, no se se revierta votación
            }
        }

        private static string BuildSummaryEmail(string citizenName, string electionName, DateTime electionDate, List<(string OfficeName, string CandidateName, string PartyName)> selections)
        {
            var items = string.Empty;
            foreach (var (officeName, candidateName, partyName) in selections)
            {
                items += $"<p><b>{officeName}</b><br>Selección: {candidateName}";
                if (!string.IsNullOrEmpty(partyName))
                    items += $"<br>Partido: {partyName}";
                items += "</p>";
            }

            return $"<html><body><p>Hola {citizenName},</p><p>Su proceso de votación ha sido completado correctamente.</p><p><b>Resumen de selección:</b></p><p>Elección: {electionName}<br>Fecha: {electionDate:dd/MM/yyyy}</p>{items}<p>Gracias por ejercer su derecho al voto.</p></body></html>";
        }
    }
}
