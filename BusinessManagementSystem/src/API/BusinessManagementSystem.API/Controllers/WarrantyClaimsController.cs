using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WarrantyClaimsController : ControllerBase
    {
        private readonly IWarrantyClaimRepository _repo;
        private readonly ILogger<WarrantyClaimsController> _logger;

        public WarrantyClaimsController(IWarrantyClaimRepository repo, ILogger<WarrantyClaimsController> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponse<IEnumerable<WarrantyClaimDto>>>> GetAll(CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            var dtos = items.Select(w => new WarrantyClaimDto(w.Id, w.OriginalWorkOrderId, w.ClaimWorkOrderId, w.Reason, w.CreatedByUserId, w.CreatedAtUtc));
            return Ok(new SuccessResponse<IEnumerable<WarrantyClaimDto>>(true, dtos));
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponse<object>>> Create([FromBody] CreateWarrantyClaimDto dto, CancellationToken ct)
        {
            try
            {
                var claim = new WarrantyClaim(dto.OriginalWorkOrderId, dto.ClaimWorkOrderId, dto.Reason, dto.CreatedByUserId);
                await _repo.AddAsync(claim, ct);
                return CreatedAtAction(nameof(GetAll), null, new SuccessResponse<object>(true, null, "Reclamo creado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando reclamo de garantía");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
