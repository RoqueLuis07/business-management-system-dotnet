using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Application.WorkOrders;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar órdenes de trabajo
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
        /// Obtiene todas las órdenes de trabajo
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetAllWorkOrders(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las órdenes de trabajo");

                var result = await GetAllWorkOrders.HandleAsync(_workOrderRepo, new GetAllWorkOrders.Query(), ct);

                var dtos = result.Select(r => new WorkOrderDto(
                    r.Id,
                    r.WorkOrderNumber,
                    r.ClientName,
                    r.EquipmentType,
                    r.Status,
                    r.CreatedAtUtc
                )).ToList();

                return Ok(new SuccessResponse<IEnumerable<WorkOrderDto>>(true, dtos, "Órdenes obtenidas"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener órdenes de trabajo");
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
                    return BadRequest(new ErrorResponse(400, "El número de OT es obligatorio"));

                _logger.LogInformation("Creando nueva orden: {WorkOrderNumber}", dto.WorkOrderNumber);

                var client = await _clientRepo.GetByIdAsync(dto.ClientId, ct);
                if (client is null)
                    return BadRequest(new ErrorResponse(400, "Cliente no encontrado"));

                // Aquí iría la lógica para obtener el Equipment
                // Por ahora, asumimos que existe

                var command = new CreateWorkOrder.Command(
                    dto.WorkOrderNumber,
                    dto.ClientId,
                    dto.EquipmentId,
                    dto.RequestedWorkDescription
                );

                await CreateWorkOrder.HandleAsync(_workOrderRepo, _clientRepo, command, ct);

                return CreatedAtAction(nameof(GetWorkOrderById), new { id = Guid.NewGuid() },
                    new SuccessResponse<object>(true, null, "Orden creada exitosamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear orden");
                return BadRequest(new ErrorResponse(400, "Error de validación", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear orden de trabajo");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Establece el diagnóstico de una orden de trabajo
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

                _logger.LogInformation("Estableciendo diagnóstico para orden {WorkOrderId}", id);

                var command = new SetWorkOrderDiagnosis.Command(
                    id,
                    dto.Findings,
                    dto.RecommendedWork,
                    dto.Notes,
                    dto.MechanicUserId
                );

                await SetWorkOrderDiagnosis.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Diagnóstico establecido"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validación", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al establecer diagnóstico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Inicia la reparación de una orden de trabajo
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

                _logger.LogInformation("Iniciando reparación para orden {WorkOrderId}", id);

                var command = new StartRepairWorkOrder.Command(id);
                await StartRepairWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Reparación iniciada"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar reparación");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Registra el reporte de servicio de una orden
        /// </summary>
        [HttpPost("{id}/service-report")]
        [ProduceResponseType(StatusCodes.Status200OK)]
        [ProduceResponseType(StatusCodes.Status404NotFound)]
        [ProduceResponseType(StatusCodes.Status400BadRequest)]
        [ProduceResponseType(StatusCodes.Status500InternalServerError)]
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
                return BadRequest(new ErrorResponse(400, "Error de validación", ex.Message));
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
        /// Obtiene órdenes de trabajo por cliente
        /// </summary>
        [HttpGet("client/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetWorkOrdersByClient(
            Guid clientId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo órdenes del cliente {ClientId}", clientId);

                var query = new GetWorkOrdersByClient.Query(clientId);
                var result = await GetWorkOrdersByClient.HandleAsync(_workOrderRepo, query, ct);

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
                _logger.LogError(ex, "Error al obtener órdenes por cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene órdenes de trabajo asignadas a un mecánico
        /// </summary>
        [HttpGet("mechanic/{mechanicId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetWorkOrdersByMechanic(
            Guid mechanicId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo órdenes del mecánico {MechanicId}", mechanicId);

                var query = new GetWorkOrdersByMechanic.Query(mechanicId);
                var result = await GetWorkOrdersByMechanic.HandleAsync(_workOrderRepo, query, ct);

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
                _logger.LogError(ex, "Error al obtener órdenes por mecánico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
