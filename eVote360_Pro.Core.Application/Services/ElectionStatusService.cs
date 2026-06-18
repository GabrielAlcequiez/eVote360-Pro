using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class ElectionStatusService : IElectionStatusService
    {
        private readonly IElectionRepository _repository;

        public ElectionStatusService(IElectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HasActiveElectionAsync()
        {
            return await _repository.ValidateNoActiveElectionAsync();
        }
    }
}
