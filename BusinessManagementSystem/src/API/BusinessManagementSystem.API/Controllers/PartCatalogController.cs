using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PartCatalogController : ControllerBase
    {
        private readonly IPartCatalogRepository _repo;
        private readonly ILogger<PartCatalogController> _logger;

        public PartCatalogController(IPartCatalogRepository repo, ILogger<PartCatalogController> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PartDto>>>> GetAll(CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            var dtos = items.Select(p => new PartDto(p.Id, p.Name, p.Description, p.DefaultUnitPrice, p.IsActive, p.CreatedAtUtc));
            return Ok(new SuccessResponse<IEnumerable<PartDto>>(true, dtos));
        }

        [HttpGet("active")]
        public async Task<ActionResult<SuccessResponse<IEnumerable<PartDto>>>> GetActive(CancellationToken ct)
        {
            var items = await _repo.GetActiveAsync(ct);
            var dtos = items.Select(p => new PartDto(p.Id, p.Name, p.Description, p.DefaultUnitPrice, p.IsActive, p.CreatedAtUtc));
            return Ok(new SuccessResponse<IEnumerable<PartDto>>(true, dtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuccessResponse<PartDto>>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _repo.GetByIdAsync(id, ct);
            if (item is null) return NotFound(new ErrorResponse(404, "Repuesto no encontrado"));
            var dto = new PartDto(item.Id, item.Name, item.Description, item.DefaultUnitPrice, item.IsActive, item.CreatedAtUtc);
            return Ok(new SuccessResponse<PartDto>(true, dto));
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<object>>> Create([FromBody] CreatePartDto dto, CancellationToken ct)
        {
            try
            {
                var existing = await _repo.GetByNameAsync(dto.Name, ct);
                if (existing is not null) return BadRequest(new ErrorResponse(400, "Repuesto ya existe"));

                var item = new PartCatalogItem(dto.Name, dto.Description, dto.DefaultUnitPrice);
                await _repo.AddAsync(item, ct);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, new SuccessResponse<object>(true, null, "Repuesto creado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando repuesto");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
