using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Application.WorkOrders;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar �rdenes de trabajo
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
        /// Obtiene todas las �rdenes de trabajo
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WorkOrderDto>>>> GetAllWorkOrders(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las �rdenes de trabajo");

                var result = await BusinessManagementSystem.Application.WorkOrders.GetAllWorkOrders.HandleAsync(_workOrderRepo, new BusinessManagementSystem.Application.WorkOrders.GetAllWorkOrders.Query(), ct);

                var dtos = result.Select(r => new WorkOrderDto(
                    r.Id,
                    r.WorkOrderNumber,
                    r.ClientName,
                    r.EquipmentType,
                    r.Status,
                    r.CreatedAtUtc
                )).ToList();

                return Ok(new SuccessResponse<IEnumerable<WorkOrderDto>>(true, dtos, "�rdenes obtenidas"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener �rdenes de trabajo");
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
                    return BadRequest(new ErrorResponse(400, "El n�mero de OT es obligatorio"));

                _logger.LogInformation("Creando nueva orden: {WorkOrderNumber}", dto.WorkOrderNumber);

                var client = await _clientRepo.GetByIdAsync(dto.ClientId, ct);
                if (client is null)
                    return BadRequest(new ErrorResponse(400, "Cliente no encontrado"));

                // Aqu� ir�a la l�gica para obtener el Equipment
                // Por ahora, asumimos que existe

                var command = new BusinessManagementSystem.Application.WorkOrders.CreateWorkOrder.Command(
                    dto.WorkOrderNumber,
                    dto.ClientFullName,
                    dto.ClientPhone,
                    dto.ClientAddress,
                    dto.EquipmentType,
                    dto.EquipmentBrand,
                    dto.EquipmentModel,
                    dto.EquipmentSerialNumber,
                    dto.RequestedWorkDescription
                );

                await BusinessManagementSystem.Application.WorkOrders.CreateWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return CreatedAtAction(nameof(GetWorkOrderById), new { id = Guid.NewGuid() },
                    new SuccessResponse<object>(true, null, "Orden creada exitosamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validaci�n al crear orden");
                return BadRequest(new ErrorResponse(400, "Error de validaci�n", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear orden de trabajo");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Establece el diagn�stico de una orden de trabajo
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

                _logger.LogInformation("Estableciendo diagn�stico para orden {WorkOrderId}", id);

                var command = new SetWorkOrderDiagnosis.Command(
                    id,
                    dto.Findings,
                    dto.RecommendedWork,
                    dto.Notes,
                    dto.MechanicUserId
                );

                await SetWorkOrderDiagnosis.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Diagn�stico establecido"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validaci�n", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al establecer diagn�stico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Inicia la reparaci�n de una orden de trabajo
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

                _logger.LogInformation("Iniciando reparaci�n para orden {WorkOrderId}", id);

                var command = new StartRepairWorkOrder.Command(id);
                await StartRepairWorkOrder.HandleAsync(_workOrderRepo, command, ct);

                return Ok(new SuccessResponse<object>(true, null, "Reparaci�n iniciada"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error operacional");
                return BadRequest(new ErrorResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar reparaci�n");
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
                return BadRequest(new ErrorResponse(400, "Error de validaci�n", ex.Message));
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
        /// Obtiene �rdenes de trabajo por cliente
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
                _logger.LogInformation("Obteniendo �rdenes del cliente {ClientId}", clientId);

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
                _logger.LogError(ex, "Error al obtener �rdenes por cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene �rdenes de trabajo asignadas a un mec�nico
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
                _logger.LogInformation("Obteniendo �rdenes del mec�nico {MechanicId}", mechanicId);

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
                _logger.LogError(ex, "Error al obtener �rdenes por mec�nico");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
