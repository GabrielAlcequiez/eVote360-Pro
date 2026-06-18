namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IElectionStatusService
    {
        Task<bool> HasActiveElectionAsync();
    }
}
