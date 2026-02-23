using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Client?> GetByPhoneAsync(string phone, CancellationToken ct);
        Task<IEnumerable<Client>> GetAllAsync(CancellationToken ct);

        Task AddAsync(Client client, CancellationToken ct);
        Task UpdateAsync(Client client, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
