using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IPartCatalogRepository
    {
        Task<PartCatalogItem?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<PartCatalogItem?> GetByNameAsync(string name, CancellationToken ct);
        Task<IEnumerable<PartCatalogItem>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<PartCatalogItem>> GetActiveAsync(CancellationToken ct);

        Task AddAsync(PartCatalogItem item, CancellationToken ct);
        Task UpdateAsync(PartCatalogItem item, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
