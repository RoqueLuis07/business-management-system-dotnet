using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class RejectWorkOrderQuote
    {
        public record Command(Guid WorkOrderId, string Reason, Guid RejectedByUserId);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.RejectQuote(cmd.Reason, cmd.RejectedByUserId);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
