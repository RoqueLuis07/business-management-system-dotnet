using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class AddPartToWorkOrder
    {
        public record Command(Guid WorkOrderId, string PartName, int Quantity);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.AddPart(cmd.PartName, cmd.Quantity);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
