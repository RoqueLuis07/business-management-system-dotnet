using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetWorkOrderByNumber
    {
        public record Query(string WorkOrderNumber);

        public record Result(
            Guid Id,
            string WorkOrderNumber,
            string ClientName,
            string EquipmentType,
            string Status,
            DateTime CreatedAtUtc);

        public static async Task<Result> HandleAsync(IWorkOrderRepository repo, Query query, CancellationToken ct)
        {
            var wo = await repo.GetByNumberAsync(query.WorkOrderNumber.Trim(), ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT con ese número.");

            return new Result(
                wo.Id,
                wo.WorkOrderNumber,
                wo.Client.FullName,
                wo.Equipment.Type,
                wo.Status.ToString(),
                wo.CreatedAtUtc);
        }
    }
}
