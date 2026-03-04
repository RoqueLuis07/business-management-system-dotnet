namespace BusinessManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO para crear/actualizar clientes
    /// </summary>
    public record CreateClientDto(
        string FullName,
        string Phone,
        string Email,
        string Address);

    /// <summary>
    /// DTO de respuesta para clientes
    /// </summary>
    public record ClientDto(
        Guid Id,
        string FullName,
        string Phone,
        string Email,
        string Address,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para crear/actualizar equipos
    /// </summary>
    public record CreateEquipmentDto(
        string Type,
        string Brand,
        string Model,
        string? SerialNumber);

    /// <summary>
    /// DTO de respuesta para equipos
    /// </summary>
    public record EquipmentDto(
        Guid Id,
        string Type,
        string Brand,
        string Model,
        string? SerialNumber,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para crear ordenes de trabajo
    /// </summary>
    public record CreateWorkOrderDto(
        string WorkOrderNumber,
        Guid ClientId,
        Guid EquipmentId,
        string RequestedWorkDescription);

    /// <summary>
    /// DTO de respuesta para ordenes de trabajo
    /// </summary>
    public record WorkOrderDto(
        Guid Id,
        string WorkOrderNumber,
        string ClientName,
        string EquipmentType,
        string Status,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para establecer diagnostico
    /// </summary>
    public record SetDiagnosisDto(
        string Findings,
        string RecommendedWork,
        string? Notes,
        Guid MechanicUserId);

    /// <summary>
    /// DTO para crear presupuesto
    /// </summary>
    public record CreateQuoteDto(
        decimal LaborCost,
        string? Notes,
        Guid CreatedByUserId);

    /// <summary>
    /// DTO para registrar reporte de servicio
    /// </summary>
    public record SetServiceReportDto(
        string WorkPerformed,
        string? Recommendations,
        string? Notes,
        Guid MechanicUserId);

    /// <summary>
    /// DTO para crear/actualizar usuarios
    /// </summary>
    public record CreateUserDto(
        string FullName,
        string Email,
        string Role,
        string? Password);

    /// <summary>
    /// DTO de respuesta para usuarios
    /// </summary>
    public record UserDto(
        Guid Id,
        string FullName,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para crear/actualizar repuestos del catalogo
    /// </summary>
    public record CreatePartDto(
        string Name,
        string? Description,
        decimal DefaultUnitPrice,
        bool IsActive);

    /// <summary>
    /// DTO de respuesta para repuestos
    /// </summary>
    public record PartDto(
        Guid Id,
        string Name,
        string? Description,
        decimal DefaultUnitPrice,
        bool IsActive,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para crear reclamo de garantia
    /// </summary>
    public record CreateWarrantyClaimDto(
        Guid OriginalWorkOrderId,
        Guid ClaimWorkOrderId,
        string Reason,
        Guid CreatedByUserId);

    /// <summary>
    /// DTO de respuesta para reclamos de garantia
    /// </summary>
    public record WarrantyClaimDto(
        Guid Id,
        Guid OriginalWorkOrderId,
        Guid ClaimWorkOrderId,
        string Reason,
        Guid CreatedByUserId,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para respuesta de error
    /// </summary>
    public record ErrorResponse(
        int StatusCode,
        string Message,
        string? Details = null);

    /// <summary>
    /// DTO para respuesta exitosa
    /// </summary>
    public record SuccessResponse<T>(
        bool Success,
        T? Data,
        string Message = "Operacion exitosa");

    // DTOs adicionales para endpoints faltantes
    public record AssignMechanicDto(Guid MechanicUserId);
    public record AddPartDto(string PartName, int Quantity);
    public record UpdatePartQuantityDto(int Quantity);
    public record PricePartDto(decimal UnitPrice, Guid? CatalogItemId);
    public record AddAccessoryDto(string Name, bool IsPresent, string? Condition);
    public record UpdateAccessoryDto(bool IsPresent, string? Condition);
    public record ApproveQuoteDto();
    public record RejectQuoteDto(string Reason, Guid RejectedByUserId);
    public record CancelWorkOrderDto(string Reason, Guid CancelledByUserId);
    public record MarkDeliveredDto(DateTime DeliveredAtLocal);
    public record SetWarrantyDaysDto(int Days);
    public record MarkAsWarrantyClaimDto(Guid OriginalWorkOrderId, string Reason, Guid CreatedByUserId);
    public record UpdatePartPriceDto(decimal NewPrice);
    public record UpdateUserNameDto(string FullName);
    public record ChangeUserRoleDto(string NewRole);
}
