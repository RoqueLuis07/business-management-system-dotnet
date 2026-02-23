using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IWarrantyClaimRepository
    {
        Task<WarrantyClaim?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<WarrantyClaim>> GetByOriginalWorkOrderAsync(Guid originalWorkOrderId, CancellationToken ct);
        Task<IEnumerable<WarrantyClaim>> GetByClaimWorkOrderAsync(Guid claimWorkOrderId, CancellationToken ct);
        Task<IEnumerable<WarrantyClaim>> GetAllAsync(CancellationToken ct);

        Task AddAsync(WarrantyClaim claim, CancellationToken ct);
        Task UpdateAsync(WarrantyClaim claim, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
