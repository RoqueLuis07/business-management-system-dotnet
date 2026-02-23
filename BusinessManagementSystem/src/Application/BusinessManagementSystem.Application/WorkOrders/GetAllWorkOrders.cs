using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetAllWorkOrders
    {
        public record Query;
        public record Result(
            Guid Id,
            string WorkOrderNumber,
            string ClientName,
            string EquipmentType,
            string Status,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IWorkOrderRepository repo, CancellationToken ct)
        {
            var workOrders = await repo.GetAllAsync(ct);
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
