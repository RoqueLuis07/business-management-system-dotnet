using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class SetWorkOrderServiceReport
    {
        public record Command(
            Guid WorkOrderId,
            string WorkPerformed,
            string? Recommendations,
            string? Notes,
            Guid MechanicUserId);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.SetServiceReport(
                cmd.WorkPerformed,
                cmd.Recommendations,
                cmd.Notes,
                cmd.MechanicUserId);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
