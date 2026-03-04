using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Application.WorkOrders;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar ordenes de trabajo
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderRepository _workOrderRepo;
        private readonly IClientRepository _clientRepo;
        private readonly ILogger<WorkOrdersController> _logger;

        public WorkOrdersController(
            IWorkOrderRepository workOrderRepo,
            IClientRepository clientRepo,
            ILogger<WorkOrdersController> logger)
        {
            _workOrderRepo = workOrderRepo ?? throw new ArgumentNullException(nameof(workOrderRepo));
            _clientRepo = clientRepo ?? throw new ArgumentNullException(nameof(clientRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las ordenes de trabajo
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetAllWorkOrders(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las ordenes de trabajo");

                var result = await BusinessManagementSystem.Application.WorkOrders.GetAllWorkOrders.HandleAsync(_workOrderRepo, new BusinessManagementSystem.Application.WorkOrders.GetAllWorkOrders.Query(), ct);

                var dtos = result.Select(r => new WorkOrderDto(
                    r.Id,
                    r.WorkOrderNumber,
                    r.ClientName,
                    r.EquipmentType,
                    r.Status,
                    r.CreatedAtUtc
                )).ToList();

                return Ok(new SuccessResponse<IEnumerable<WorkOrderDto>>(true, dtos, "Ordenes obtenidas"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ordenes de trabajo");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene una orden de trabajo por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<WorkOrderDto>>> GetWorkOrderById(Guid id, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo orden {WorkOrderId}", id);

                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                {
                    _logger.LogWarning("Orden {WorkOrderId} no encontrada", id);
                    return NotFound(new ErrorResponse(404, "Orden de trabajo no encontrada"));
                }

                var dto = new WorkOrderDto(
                    workOrder.Id,
                    workOrder.WorkOrderNumber,
                    workOrder.Client.FullName,
                    workOrder.Equipment.Type,
                    workOrder.Status.ToString(),
                    workOrder.CreatedAtUtc
                );

                return Ok(new SuccessResponse<WorkOrderDto>(true, dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener orden de trabajo");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene una orden de trabajo por su numero
        /// </summary>
        [HttpGet("number/{number}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse<WorkOrderDto>>> GetWorkOrderByNumber(string number, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo orden por numero: {WorkOrderNumber}", number);

                var query = new GetWorkOrderByNumber.Query(number);
                var result = await GetWorkOrderByNumber.HandleAsync(_workOrderRepo, query, ct);
                
                if (result is null)
                    return NotFound(new ErrorResponse(404, "Orden de trabajo no encontrada"));

                var dto = new WorkOrderDto(
                    result.Id,
                    result.WorkOrderNumber,
                    result.ClientName,
                    result.EquipmentType,
                    result.Status,
                    result.CreatedAtUtc
                );

                return Ok(new SuccessResponse<WorkOrderDto>(true, dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener orden por numero");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene ordenes de trabajo por estado
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetWorkOrdersByStatus(string status, CancellationToken ct)
        {
            try
            {
                if (!Enum.TryParse<WorkOrderStatus>(status, true, out var workOrderStatus))
                    return BadRequest(new ErrorResponse(400, "Estado invalido"));

                _logger.LogInformation("Obteniendo ordenes con estado: {Status}", status);

                var query = new GetWorkOrdersByStatus.Query(workOrderStatus);
                var result = await GetWorkOrdersByStatus.HandleAsync(_workOrderRepo, query, ct);

                var dtos = result.Select(r => new WorkOrderDto(
                    r.Id,
                    r.WorkOrderNumber,
                    r.ClientName,
                    r.EquipmentType,
                    r.Status,
                    r.CreatedAtUtc
                )).ToList();

                return Ok(new SuccessResponse<IEnumerable<WorkOrderDto>>(true, dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ordenes por estado");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene ordenes de trabajo en periodo de garantia
        /// </summary>
        [HttpGet("warranty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetWorkOrdersUnderWarranty(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo ordenes en garantia");

                var query = new GetWorkOrdersUnderWarranty.Query();
                var result = await GetWorkOrdersUnderWarranty.HandleAsync(_workOrderRepo, query, ct);

                var dtos = result.Select(r => new WorkOrderDto(
                    r.Id,
                    r.WorkOrderNumber,
                    r.ClientName,
                    r.EquipmentType,
                    r.Status,
                    r.CreatedAtUtc
                )).ToList();

                return Ok(new SuccessResponse<IEnumerable<WorkOrderDto>>(true, dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ordenes en garantia");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Crea una nueva orden de trabajo
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> CreateWorkOrder(
            [FromBody] CreateWorkOrderDto dto,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.WorkOrderNumber))
                    return BadRequest(new ErrorResponse(400, "El numero de OT es obligatorio"));

                _logger.LogInformation("Creando nueva orden: {WorkOrderNumber}", dto.WorkOrderNumber);

                var client = await _clientRepo.GetByIdAsync(dto.ClientId, ct);
                if (client is null)
                    return BadRequest(new ErrorResponse(400, "Cliente no encontrado"));

                var command = new BusinessManagementSystem.Application.WorkOrders.CreateWorkOrder.Command(
                    dto.WorkOrderNumber,
                    client.FullName,
                    client.Phone,
                    client.Address,
                    "Equipo", // Default type - would need EquipmentId lookup
                    null,
                    null,
                    null,
                    dto.RequestedWorkDescription
                );

                var workOrderId = await BusinessManagementSystem.Application.WorkOrders.CreateWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return CreatedAtAction(nameof(GetWorkOrderById), new { id = workOrderId },
                    new SuccessResponse<object>(true, null, "Orden creada exitosamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validacion al crear orden");
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear orden de trabajo");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Asigna un mecanico a una orden de trabajo
        /// </summary>
        [HttpPost("{id}/assign-mechanic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> AssignMechanic(Guid id, [FromBody] AssignMechanicDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Asignando mecanico a orden {WorkOrderId}", id);

                var command = new AssignMechanicToWorkOrder.Command(id, dto.MechanicUserId);
                await AssignMechanicToWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Mecanico asignado"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar mecanico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Inicia el diagnostico de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/start-diagnosis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> StartDiagnosis(Guid id, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Iniciando diagnostico para orden {WorkOrderId}", id);

                var command = new StartWorkOrderDiagnosis.Command(id);
                await StartWorkOrderDiagnosis.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Diagnostico iniciado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar diagnostico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Establece el diagnostico de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/diagnosis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> SetDiagnosis(
            Guid id,
            [FromBody] SetDiagnosisDto dto,
            CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Estableciendo diagnostico para orden {WorkOrderId}", id);

                var command = new SetWorkOrderDiagnosis.Command(
                    id,
                    dto.Findings,
                    dto.RecommendedWork,
                    dto.Notes,
                    dto.MechanicUserId
                );

                await SetWorkOrderDiagnosis.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Diagnostico establecido"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al establecer diagnostico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Genera un presupuesto para la orden de trabajo
        /// </summary>
        [HttpPost("{id}/quote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> GenerateQuote(Guid id, [FromBody] CreateQuoteDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Generando presupuesto para orden {WorkOrderId}", id);

                var command = new GenerateWorkOrderQuote.Command(id, dto.LaborCost, dto.Notes, dto.CreatedByUserId);
                await GenerateWorkOrderQuote.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Presupuesto generado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar presupuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Aprueba el presupuesto de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> ApproveQuote(Guid id, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Aprobando presupuesto para orden {WorkOrderId}", id);

                var command = new ApproveWorkOrder.Command(id);
                await ApproveWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Presupuesto aprobado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aprobar presupuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Rechaza el presupuesto de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> RejectQuote(Guid id, [FromBody] RejectQuoteDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Rechazando presupuesto para orden {WorkOrderId}", id);

                var command = new RejectWorkOrderQuote.Command(id, dto.Reason, dto.RejectedByUserId);
                await RejectWorkOrderQuote.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Presupuesto rechazado"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al rechazar presupuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Cancela una orden de trabajo
        /// </summary>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> CancelWorkOrder(Guid id, [FromBody] CancelWorkOrderDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Cancelando orden {WorkOrderId}", id);

                var command = new CancelWorkOrder.Command(id, dto.Reason, dto.CancelledByUserId);
                await CancelWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Orden cancelada"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar orden");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Inicia la reparacion de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/start-repair")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> StartRepair(Guid id, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Iniciando reparacion para orden {WorkOrderId}", id);

                var command = new StartRepairWorkOrder.Command(id);
                await StartRepairWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Reparacion iniciada"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar reparacion");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Registra el reporte de servicio de una orden
        /// </summary>
        [HttpPost("{id}/service-report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> SetServiceReport(
            Guid id,
            [FromBody] SetServiceReportDto dto,
            CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Registrando reporte de servicio para orden {WorkOrderId}", id);

                var command = new SetWorkOrderServiceReport.Command(
                    id,
                    dto.WorkPerformed,
                    dto.Recommendations,
                    dto.Notes,
                    dto.MechanicUserId
                );

                await SetWorkOrderServiceReport.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Reporte registrado"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar reporte");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Marca una orden como terminada
        /// </summary>
        [HttpPost("{id}/finish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> MarkFinished(Guid id, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Marcando orden {WorkOrderId} como terminada", id);

                var command = new MarkWorkOrderFinished.Command(id);
                await MarkWorkOrderFinished.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Orden marcada como terminada"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar orden como terminada");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Marca una orden como lista para entrega
        /// </summary>
        [HttpPost("{id}/ready-for-delivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> MarkReadyForDelivery(Guid id, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Marcando orden {WorkOrderId} como lista para entrega", id);

                var command = new MarkWorkOrderReadyForDelivery.Command(id);
                await MarkWorkOrderReadyForDelivery.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Orden lista para entrega"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar orden como lista para entrega");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Marca una orden como entregada
        /// </summary>
        [HttpPost("{id}/deliver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> MarkDelivered(Guid id, [FromBody] MarkDeliveredDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Marcando orden {WorkOrderId} como entregada", id);

                var command = new MarkWorkOrderDelivered.Command(id, dto.DeliveredAtLocal);
                await MarkWorkOrderDelivered.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Orden entregada"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validacion", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar orden como entregada");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Agrega un repuesto a una orden de trabajo
        /// </summary>
        [HttpPost("{id}/parts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> AddPart(Guid id, [FromBody] AddPartDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Agregando repuesto a orden {WorkOrderId}", id);

                var command = new AddPartToWorkOrder.Command(id, dto.PartName, dto.Quantity);
                await AddPartToWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Repuesto agregado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Precifica un repuesto de una orden de trabajo
        /// </summary>
        [HttpPost("{id}/parts/{partId}/price")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> PricePart(Guid id, Guid partId, [FromBody] PricePartDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Precificando repuesto {PartId} en orden {WorkOrderId}", partId, id);

                var command = new PriceWorkOrderPart.Command(id, partId, dto.UnitPrice, dto.CatalogItemId);
                await PriceWorkOrderPart.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Repuesto precificado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al precificar repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un repuesto
        /// </summary>
        [HttpPut("{id}/parts/{partId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> UpdatePartQuantity(Guid id, Guid partId, [FromBody] UpdatePartQuantityDto dto, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Actualizando cantidad de repuesto {PartId} en orden {WorkOrderId}", partId, id);

                var command = new UpdateWorkOrderPartQuantity.Command(id, partId, dto.Quantity);
                await UpdateWorkOrderPartQuantity.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Cantidad actualizada"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cantidad de repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Elimina un repuesto de una orden de trabajo
        /// </summary>
        [HttpDelete("{id}/parts/{partId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> RemovePart(Guid id, Guid partId, CancellationToken ct)
        {
            try
            {
                var workOrder = await _workOrderRepo.GetByIdAsync(id, ct);
                if (workOrder is null)
                    return NotFound(new ErrorResponse(404, "Orden no encontrada"));

                _logger.LogInformation("Eliminando repuesto {PartId} de orden {WorkOrderId}", partId, id);

                var command = new RemovePartFromWorkOrder.Command(id, partId);
                await RemovePartFromWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Repuesto eliminado"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Agrega un accesorio a una orden de trabajo
        /// </summary>
