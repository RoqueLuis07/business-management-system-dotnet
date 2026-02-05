using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class CreateWorkOrder
    {
        public record Command(
            string WorkOrderNumber,
            string ClientFullName,
            string ClientPhone,
            string ClientAddress,
            string EquipmentType,
            string? EquipmentBrand,
            string? EquipmentModel,
            string? EquipmentSerialNumber,
            string RequestedWorkDescription);

        public static async Task<Guid> HandleAsync(IWorkOrderRepository repo, Command cmd, CancellationToken ct)
        {
            // Regla de negocio: el número de OT es único global.
            var existing = await repo.GetByNumberAsync(cmd.WorkOrderNumber.Trim(), ct);
            if (existing is not null)
                throw new InvalidOperationException("Ya existe una OT con ese número.");

            var client = new Client(cmd.ClientFullName, cmd.ClientPhone, cmd.ClientAddress);
            var equipment = new Equipment(cmd.EquipmentType, cmd.EquipmentBrand, cmd.EquipmentModel, cmd.EquipmentSerialNumber);

            var wo = new WorkOrder(cmd.WorkOrderNumber, client, equipment, cmd.RequestedWorkDescription);

            await repo.AddAsync(wo, ct);
            return wo.Id;
        }
    }
}
