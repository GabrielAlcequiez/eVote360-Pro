using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PartyLeader;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class PartyLeaderService(
        IPartyLeaderRepository partyLeaderRepository,
        IElectionRepository electionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPartyLeaderService
    {
        private readonly IPartyLeaderRepository _partyLeaderRepository = partyLeaderRepository;
        private readonly IElectionRepository _electionRepository = electionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AssignAsync(PartyLeaderCreateDto dto)
        {
            await ValidateNoActiveElectionAsync("crear una asignacion de");
            if (dto.UserId == Guid.Empty)
                throw new ArgumentException("Debe seleccionar un dirigente político.");

            if (dto.PoliticalPartyId == Guid.Empty)
                throw new ArgumentException("Debe seleccionar un partido político.");

            var allLeaders = await _partyLeaderRepository.GetAllWithDetailsAsync();
            bool leaderAlreadyAssigned = allLeaders.Any(pl => pl.UserId == dto.UserId);

            if (leaderAlreadyAssigned)
                throw new InvalidOperationException("Este dirigente ya está relacionado con otro partido político.");

            bool partyAlreadyAssigned = allLeaders.Any(pl => pl.PoliticalPartyId == dto.PoliticalPartyId);

            if (partyAlreadyAssigned)
                throw new InvalidOperationException("Este partido político ya tiene un dirigente asignado.");

            var entity = new PartyLeader(dto.UserId, dto.PoliticalPartyId);

            await _partyLeaderRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

        }

        public async Task DeleteAsync(Guid userId)
        {
            await ValidateNoActiveElectionAsync("eliminar asignacion de");

            var allLeaders = await _partyLeaderRepository.GetAllWithDetailsAsync();
            bool exists = allLeaders.Any(pl => pl.UserId == userId);

            if (!exists)
                throw new KeyNotFoundException("La asignación seleccionada no existe o ya fue eliminada.");

            await _partyLeaderRepository.DeletePhysicallyAsync(userId);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<PartyLeaderGetDto>> GetAllAsync()
        {
            var list = await _partyLeaderRepository.GetAllWithDetailsAsync();
            return _mapper.Map<List<PartyLeaderGetDto>>(list);
        }

        public async Task<List<UserGetDto>> GetAvailableLeadersAsync()
        {
            var users = await _partyLeaderRepository.GetAvailableLeadersAsync();
            return _mapper.Map<List<UserGetDto>>(users);
        }

        public async Task<List<PoliticalPartyGetDto>> GetAvailablePartiesAsync()
        {
            var parties = await _partyLeaderRepository.GetAvailablePartiesAsync();
            return _mapper.Map<List<PoliticalPartyGetDto>>(parties);
        }


        #region Helpers
        private async Task ValidateNoActiveElectionAsync(string action)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
            {
                throw new InvalidOperationException(
                  $"No se puede {action} dirigente político mientras exista una elección activa.");
            }
        }
        #endregion
    }
}