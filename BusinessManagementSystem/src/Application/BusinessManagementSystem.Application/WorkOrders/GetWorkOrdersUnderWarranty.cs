using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetWorkOrdersUnderWarranty
    {
        public record Query(DateTime NowLocal);
        public record Result(
            Guid Id,
            string WorkOrderNumber,
            string ClientName,
            DateTime? DeliveredAtLocal,
            int WarrantyDays);

        public static async Task<IEnumerable<Result>> HandleAsync(IWorkOrderRepository repo, Query query, CancellationToken ct)
        {
            var workOrders = await repo.GetUnderWarrantyAsync(query.NowLocal, ct);
            return workOrders.Select(wo => new Result(
                wo.Id,
                wo.WorkOrderNumber,
                wo.Client.FullName,
                wo.DeliveredAtLocal,
                wo.WarrantyDays));
        }
    }
}
