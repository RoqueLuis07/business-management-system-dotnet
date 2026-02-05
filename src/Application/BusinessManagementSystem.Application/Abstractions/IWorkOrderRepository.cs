using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IWorkOrderRepository
    {
        Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<WorkOrder?> GetByNumberAsync(string workOrderNumber, CancellationToken ct);

        Task AddAsync(WorkOrder workOrder, CancellationToken ct);
        Task UpdateAsync(WorkOrder workOrder, CancellationToken ct);
    }
}
