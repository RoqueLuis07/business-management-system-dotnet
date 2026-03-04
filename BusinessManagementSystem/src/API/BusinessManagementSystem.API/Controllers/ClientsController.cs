using Microsoft.AspNetCore.Mvc;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Application.Clients;
using BusinessManagementSystem.API.DTOs;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller para gestionar clientes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repository;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientRepository repository, ILogger<ClientsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAllClients(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los clientes");

                var result = await BusinessManagementSystem.Application.Clients.GetAllClients.HandleAsync(_repository, new BusinessManagementSystem.Application.Clients.GetAllClients.Query(), ct);

                var dtos = result.Select(r => new ClientDto(
                    r.Id,
                    r.FullName,
                    r.Phone,
                    r.Address,
                    r.Address,
                    DateTime.UtcNow
                ));

                return Ok(new SuccessResponse<IEnumerable<ClientDto>>(true, dtos, "Clientes obtenidos"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        /// <param name="id">ID del cliente</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientDto>> GetClientById(Guid id, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Obteniendo cliente {ClientId}", id);

                var client = await _repository.GetByIdAsync(id, ct);
                if (client is null)
                {
                    _logger.LogWarning("Cliente {ClientId} no encontrado", id);
                    return NotFound(new ErrorResponse(404, "Cliente no encontrado"));
                }

                var dto = new ClientDto(
                    client.Id,
                    client.FullName,
                    client.Phone,
                    client.Email,
                    client.Address,
                    DateTime.UtcNow
                );

                return Ok(new SuccessResponse<ClientDto>(true, dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="dto">Datos del cliente a crear</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> CreateClient(
            [FromBody] CreateClientDto dto,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    return BadRequest(new ErrorResponse(400, "El nombre del cliente es obligatorio"));

                _logger.LogInformation("Creando nuevo cliente: {ClientName}", dto.FullName);

                var command = new BusinessManagementSystem.Application.Clients.CreateClient.Command(
                    dto.FullName,
                    dto.Phone,
                    dto.Address
                );

                await BusinessManagementSystem.Application.Clients.CreateClient.HandleAsync(_repository, command, ct);

                return CreatedAtAction(nameof(GetClientById), new { id = Guid.NewGuid() },
                    new SuccessResponse<object>(true, null, "Cliente creado exitosamente"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validaci�n al crear cliente");
                return BadRequest(new ErrorResponse(400, "Error de validaci�n", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="dto">Datos actualizados</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> UpdateClient(
            Guid id,
            [FromBody] CreateClientDto dto,
            CancellationToken ct)
        {
            try
            {
                var client = await _repository.GetByIdAsync(id, ct);
                if (client is null)
                    return NotFound(new ErrorResponse(404, "Cliente no encontrado"));

                _logger.LogInformation("Actualizando cliente {ClientId}", id);

                client.UpdateInfo(dto.FullName, dto.Phone, dto.Address);
                await _repository.UpdateAsync(client, ct);

                return Ok(new SuccessResponse<object>(true, null, "Cliente actualizado"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Error de validaci�n", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SuccessResponse<object>>> DeleteClient(Guid id, CancellationToken ct)
        {
            try
            {
                var client = await _repository.GetByIdAsync(id, ct);
                if (client is null)
                    return NotFound(new ErrorResponse(404, "Cliente no encontrado"));

                _logger.LogInformation("Eliminando cliente {ClientId}", id);

                await _repository.DeleteAsync(id, ct);

                return Ok(new SuccessResponse<object>(true, null, "Cliente eliminado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente");
                return StatusCode(500, new ErrorResponse(500, "Error interno del servidor", ex.Message));
            }
        }
    }
}
