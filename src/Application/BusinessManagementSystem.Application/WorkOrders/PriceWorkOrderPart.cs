using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class PriceWorkOrderPart
    {
        public record Command(
            Guid WorkOrderId,
            Guid WorkOrderPartId,
            decimal UnitPrice,
            Guid? CatalogItemId = null);

        public static async Task HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(cmd.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            wo.PricePart(cmd.WorkOrderPartId, cmd.UnitPrice, cmd.CatalogItemId);

            await repo.UpdateAsync(wo, ct);
        }
    }
}
