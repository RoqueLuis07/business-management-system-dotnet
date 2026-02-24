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
    /// DTO para crear órdenes de trabajo
    /// </summary>
    public record CreateWorkOrderDto(
        string WorkOrderNumber,
        Guid ClientId,
        Guid EquipmentId,
        string RequestedWorkDescription);

    /// <summary>
    /// DTO de respuesta para órdenes de trabajo
    /// </summary>
    public record WorkOrderDto(
        Guid Id,
        string WorkOrderNumber,
        string ClientName,
        string EquipmentType,
        string Status,
        DateTime CreatedAtUtc);

    /// <summary>
    /// DTO para establecer diagnóstico
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
        string Message = "Operación exitosa");
}
