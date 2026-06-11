using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IElectedOfficeRepository : IBaseRepository<ElectedOffice>
    {
        Task<ElectedOffice?> GetByName(string name);
        Task<bool> HasBeenUsedInElectionAsync(Guid id);
        Task<bool> HasActiveCandidatesAssignedAsync(Guid id);
    }
}