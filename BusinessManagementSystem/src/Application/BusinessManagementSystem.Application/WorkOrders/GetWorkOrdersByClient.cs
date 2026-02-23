using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetWorkOrdersByClient
    {
        public record Query(Guid ClientId);
        public record Result(
            Guid Id,
            string WorkOrderNumber,
            string EquipmentType,
            string Status,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IWorkOrderRepository repo, Query query, CancellationToken ct)
        {
            var workOrders = await repo.GetByClientAsync(query.ClientId, ct);
            return workOrders.Select(wo => new Result(
                wo.Id,
                wo.WorkOrderNumber,
                wo.Equipment.Type,
                wo.Status.ToString(),
                wo.CreatedAtUtc));
        }
    }
}
