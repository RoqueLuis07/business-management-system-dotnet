using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.API.DTOs;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository repository, ILogger<UsersController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetAllUsers(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios");
                var users = await _repository.GetAllAsync(ct);
                return Ok(new SuccessResponse<object>(true, users, "Usuarios obtenidos"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetUserById(Guid id, CancellationToken ct)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id, ct);
                if (user is null)
                    return NotFound(new ErrorResponse(404, "Usuario no encontrado"));

                return Ok(new SuccessResponse<object>(true, user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetUserByEmail(string email, CancellationToken ct)
        {
            try
            {
                var user = await _repository.GetByEmailAsync(email, ct);
                if (user is null)
                    return NotFound(new ErrorResponse(404, "Usuario no encontrado"));

                return Ok(new SuccessResponse<object>(true, user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene solo usuarios activos
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> GetActiveUsers(CancellationToken ct)
        {
            try
            {
                var users = await _repository.GetActiveUsersAsync(ct);
                return Ok(new SuccessResponse<object>(true, users, "Usuarios activos obtenidos"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios activos");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
