namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // acá iran todos los repository

        Task<int> CompleteAsync();
    }
}