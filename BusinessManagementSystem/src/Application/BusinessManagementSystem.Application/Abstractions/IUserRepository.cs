using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role, CancellationToken ct);
        Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct);

        Task AddAsync(User user, CancellationToken ct);
        Task UpdateAsync(User user, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
