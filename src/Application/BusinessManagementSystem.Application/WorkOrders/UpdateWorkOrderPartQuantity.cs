using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class UpdateWorkOrderPartQuantity
    {
        public record Command(Guid WorkOrderId, Guid WorkOrderPartId, int Quantity);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.UpdatePartQuantity(cmd.WorkOrderPartId, cmd.Quantity);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
