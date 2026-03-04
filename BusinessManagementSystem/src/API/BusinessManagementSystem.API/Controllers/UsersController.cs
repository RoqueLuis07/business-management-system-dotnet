using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository repo, ILogger<UsersController> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<IEnumerable<UserDto>>>> GetAll(CancellationToken ct)
        {
            try
            {
                var users = await _repo.GetAllAsync(ct);
                var dtos = users.Select(u => new UserDto(u.Id, u.FullName, u.Email, u.Role.ToString(), u.IsActive, u.CreatedAtUtc));
                return Ok(new SuccessResponse<IEnumerable<UserDto>>(true, dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse<UserDto>>> GetById(Guid id, CancellationToken ct)
        {
            var user = await _repo.GetByIdAsync(id, ct);
            if (user is null) return NotFound(new ErrorResponse(404, "Usuario no encontrado"));
            var dto = new UserDto(user.Id, user.FullName, user.Email, user.Role.ToString(), user.IsActive, user.CreatedAtUtc);
            return Ok(new SuccessResponse<UserDto>(true, dto));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuccessResponse<object>>> Create([FromBody] CreateUserDto dto, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new ErrorResponse(400, "Nombre y email obligatorios"));

                if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                    return BadRequest(new ErrorResponse(400, "Rol inválido"));

                var exists = await _repo.GetByEmailAsync(dto.Email, ct);
                if (exists is not null) return BadRequest(new ErrorResponse(400, "Email ya registrado"));

                var user = new User(dto.FullName, dto.Email, role, dto.Password);
                await _repo.AddAsync(user, ct);

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, new SuccessResponse<object>(true, null, "Usuario creado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SuccessResponse<object>>> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                await _repo.DeleteAsync(id, ct);
                return Ok(new SuccessResponse<object>(true, null, "Usuario eliminado"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ErrorResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando usuario");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
