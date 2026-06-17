using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface ICitizenRepository : IBaseRepository<Citizen>
    {
        Task<Citizen?> GetByName(string name);
        Task<Citizen?> GetByEmail(string email);
        Task<Citizen?> GetByDocumentNumber(string documentNumber);

        Task<bool> HasBeenUsedInElectionAsync(Guid id, Guid electionId);
        Task<bool> HasBeenUsedInAnyElectionAsync(Guid id);
        Task AddParticipationAsync(CitizenParticipation participation);
    }
}