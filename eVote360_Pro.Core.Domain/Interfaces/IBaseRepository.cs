namespace eVote360_Pro.Core.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>?> AddRangeAsync(List<T> entities);
        Task<T?> UpdateAsync(Guid id, T entity);
        // Soft por requerimiento
        Task<T?> SoftDeleteAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        IQueryable<T> AsQueryable();
    }
}