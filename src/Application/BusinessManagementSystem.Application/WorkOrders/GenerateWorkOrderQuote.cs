using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GenerateWorkOrderQuote
    {
        public record Command(
            Guid WorkOrderId,
            decimal LaborCost,
            string? Notes,
            Guid CreatedByUserId);

        public static async Task HandleAsync(
            IWorkOrderRepository repo,
            Command cmd,
            CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.CreateOrUpdateQuote(
                cmd.LaborCost,
                cmd.Notes,
                cmd.CreatedByUserId);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
