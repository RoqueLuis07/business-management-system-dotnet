using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class RemovePartFromWorkOrder
    {
        public record Command(Guid WorkOrderId, Guid WorkOrderPartId);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.RemovePart(cmd.WorkOrderPartId);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
