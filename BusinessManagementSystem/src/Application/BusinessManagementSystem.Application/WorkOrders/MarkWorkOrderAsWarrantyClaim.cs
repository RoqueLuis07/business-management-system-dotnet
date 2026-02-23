using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class MarkWorkOrderAsWarrantyClaim
    {
        public record Command(
            Guid WorkOrderId,
            Guid OriginalWorkOrderId,
            string Reason,
            Guid CreatedByUserId,
            DateTime NowLocal);

        public static async Task HandleAsync(
            IWorkOrderRepository repo,
            Command cmd,
            CancellationToken ct)
        {
            var workOrder = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (workOrder is null)
                throw new InvalidOperationException("No se encontró la OT de garantía.");

            var originalWorkOrder = await repo.GetByIdAsync(cmd.OriginalWorkOrderId, ct);
            if (originalWorkOrder is null)
                throw new InvalidOperationException("No se encontró la OT original.");

            workOrder.MarkAsWarrantyClaimOf(
                originalWorkOrder,
                cmd.Reason,
                cmd.CreatedByUserId,
                cmd.NowLocal);

            await repo.UpdateAsync(workOrder, ct);
        }
    }
}
