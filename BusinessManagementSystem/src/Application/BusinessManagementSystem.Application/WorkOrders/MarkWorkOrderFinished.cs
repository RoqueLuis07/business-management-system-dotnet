using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class MarkWorkOrderFinished
    {
        public record Command(Guid WorkOrderId);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.MarkFinished();

            await repo.UpdateAsync(wo, ct);
        }
    }
}
