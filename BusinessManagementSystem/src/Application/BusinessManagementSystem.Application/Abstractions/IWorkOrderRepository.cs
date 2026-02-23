using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Application.Abstractions
{
    public interface IWorkOrderRepository
    {
        Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<WorkOrder?> GetByNumberAsync(string workOrderNumber, CancellationToken ct);
        Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken ct);
        Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct);
        Task<IEnumerable<WorkOrder>> GetByClientAsync(Guid clientId, CancellationToken ct);
        Task<IEnumerable<WorkOrder>> GetByMechanicAsync(Guid mechanicUserId, CancellationToken ct);
        Task<IEnumerable<WorkOrder>> GetUnderWarrantyAsync(DateTime nowLocal, CancellationToken ct);

        Task AddAsync(WorkOrder workOrder, CancellationToken ct);
        Task UpdateAsync(WorkOrder workOrder, CancellationToken ct);
    }
}
