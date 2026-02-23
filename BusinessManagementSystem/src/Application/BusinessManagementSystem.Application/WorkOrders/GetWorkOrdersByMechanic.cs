using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetWorkOrdersByMechanic
    {
        public record Query(Guid MechanicUserId);
        public record Result(
            Guid Id,
            string WorkOrderNumber,
            string ClientName,
            string EquipmentType,
            string Status,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IWorkOrderRepository repo, Query query, CancellationToken ct)
        {
            var workOrders = await repo.GetByMechanicAsync(query.MechanicUserId, ct);
            return workOrders.Select(wo => new Result(
                wo.Id,
                wo.WorkOrderNumber,
                wo.Client.FullName,
                wo.Equipment.Type,
                wo.Status.ToString(),
                wo.CreatedAtUtc));
        }
    }
}
