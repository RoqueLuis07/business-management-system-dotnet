using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class AddAccessoryToWorkOrder
    {
        public record Command(
            Guid WorkOrderId,
            string AccessoryName,
            bool IsPresent,
            string? Condition);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.AddAccessory(cmd.AccessoryName, cmd.IsPresent, cmd.Condition);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
