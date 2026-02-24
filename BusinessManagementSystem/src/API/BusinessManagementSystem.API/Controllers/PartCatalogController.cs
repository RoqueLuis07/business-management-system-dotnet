using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.API.DTOs;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar catálogo de repuestos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PartCatalogController : ControllerBase
    {
        private readonly IPartCatalogRepository _repository;
        private readonly ILogger<PartCatalogController> _logger;

        public PartCatalogController(IPartCatalogRepository repository, ILogger<PartCatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los repuestos
        /// </summary>
        [HttpGet]
        [ProduceResponseType(StatusCodes.Status200OK)]
        [ProduceResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetAllParts(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo catálogo de repuestos");
                var parts = await _repository.GetAllAsync(ct);
                return Ok(new SuccessResponse<object>(true, parts, "Repuestos obtenidos"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuestos");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene solo repuestos activos
        /// </summary>
        [HttpGet("active")]
        [ProduceResponseType(StatusCodes.Status200OK)]
        [ProduceResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetActiveParts(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo repuestos activos");
                var parts = await _repository.GetActiveAsync(ct);
                return Ok(new SuccessResponse<object>(true, parts, "Repuestos activos obtenidos"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuestos activos");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un repuesto por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProduceResponseType(StatusCodes.Status200OK)]
        [ProduceResponseType(StatusCodes.Status404NotFound)]
        [ProduceResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetPartById(Guid id, CancellationToken ct)
        {
            try
            {
                var part = await _repository.GetByIdAsync(id, ct);
                if (part is null)
                    return NotFound(new ErrorResponse(404, "Repuesto no encontrado"));

                return Ok(new SuccessResponse<object>(true, part));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un repuesto por su nombre
        /// </summary>
        [HttpGet("name/{name}")]
        [ProduceResponseType(StatusCodes.Status200OK)]
        [ProduceResponseType(StatusCodes.Status404NotFound)]
        [ProduceResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetPartByName(string name, CancellationToken ct)
        {
            try
            {
                var part = await _repository.GetByNameAsync(name, ct);
                if (part is null)
                    return NotFound(new ErrorResponse(404, "Repuesto no encontrado"));

                return Ok(new SuccessResponse<object>(true, part));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
