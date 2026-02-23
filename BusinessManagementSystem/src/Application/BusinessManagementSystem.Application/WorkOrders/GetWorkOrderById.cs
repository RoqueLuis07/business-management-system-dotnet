using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.WorkOrders
{
    public static class GetWorkOrderById
    {
        public record Query(Guid WorkOrderId);

        public record Result(
            Guid Id,
            string WorkOrderNumber,
            ClientDto Client,
            EquipmentDto Equipment,
            string RequestedWorkDescription,
            string Status,
            DiagnosisDto? Diagnosis,
            QuoteDto? Quote,
            ServiceReportDto? ServiceReport,
            DateTime? DeliveredAtLocal,
            int WarrantyDays,
            Guid? AssignedMechanicUserId,
            IEnumerable<AccessoryDto> Accessories,
            IEnumerable<PartDto> Parts);

        public record ClientDto(Guid Id, string FullName, string Phone, string Address);
        public record EquipmentDto(string Type, string Brand, string Model, string SerialNumber, bool IsIdentified);
        public record DiagnosisDto(string Findings, string RecommendedWork, string? Notes, Guid MechanicUserId);
        public record QuoteDto(decimal LaborCost, decimal PartsTotal, decimal Total, string? Notes, Guid CreatedByUserId);
        public record ServiceReportDto(string WorkPerformed, string? Recommendations, string? Notes, Guid MechanicUserId);
        public record AccessoryDto(Guid Id, string Name, bool IsPresent, string? Condition);
        public record PartDto(Guid Id, string PartName, int Quantity, decimal? UnitPrice, decimal? LineTotal);

        public static async Task<Result> HandleAsync(IWorkOrderRepository repo, Query query, CancellationToken ct)
        {
            var wo = await repo.GetByIdAsync(query.WorkOrderId, ct);
            if (wo is null)
                throw new InvalidOperationException("No se encontró la OT.");

            return MapToResult(wo);
        }

        private static Result MapToResult(WorkOrder wo) =>
            new Result(
                wo.Id,
                wo.WorkOrderNumber,
                new ClientDto(wo.Client.Id, wo.Client.FullName, wo.Client.Phone, wo.Client.Address),
                new EquipmentDto(wo.Equipment.Type, wo.Equipment.Brand, wo.Equipment.Model, wo.Equipment.SerialNumber, wo.Equipment.IsIdentified),
                wo.RequestedWorkDescription,
                wo.Status.ToString(),
                wo.Diagnosis is not null
                    ? new DiagnosisDto(wo.Diagnosis.Findings, wo.Diagnosis.RecommendedWork, wo.Diagnosis.Notes, wo.Diagnosis.MechanicUserId)
                    : null,
                wo.Quote is not null
                    ? new QuoteDto(wo.Quote.LaborCost, wo.Quote.PartsTotal, wo.Quote.Total, wo.Quote.Notes, wo.Quote.CreatedByUserId)
                    : null,
                wo.ServiceReport is not null
                    ? new ServiceReportDto(wo.ServiceReport.WorkPerformed, wo.ServiceReport.Recommendations, wo.ServiceReport.Notes, wo.ServiceReport.MechanicUserId)
                    : null,
                wo.DeliveredAtLocal,
                wo.WarrantyDays,
                wo.AssignedMechanicUserId,
                wo.Accessories.Select(a => new AccessoryDto(a.Id, a.Name, a.IsPresent, a.Condition)),
                wo.Parts.Select(p => new PartDto(p.Id, p.PartName, p.Quantity, p.UnitPrice, p.LineTotal)));
    }
}
