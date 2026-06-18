using eVote360_Pro.Core.Application.DTOs.User;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IUserService
    {
        // Método de autenticación (Login)
        Task<UserGetDto?> LoginAsync(string username, string password);

        // CRUD del mantenimiento de usuarios
        Task<UserGetDto> AddAsync(UserCreateDto dto);
        Task UpdateAsync(UserUpdateDto dto);
        Task DeleteAsync(Guid id, Guid currentUserId);
        Task<UserGetDto?> GetByIdAsync(Guid id);
        Task<UserGetDto?> GetByUsernameAsync(string username);
        Task<List<UserGetDto>> GetAllAsync();
    }
}