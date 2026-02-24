using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Infrastructure.Data;

namespace BusinessManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IClientRepository usando Entity Framework Core
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        /// <summary>
        /// Obtiene un cliente por su teléfono (único)
        /// </summary>
        public async Task<Client?> GetByPhoneAsync(string phone, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Phone == phone.Trim(), ct);
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Clients
                .AsNoTracking()
                .OrderBy(c => c.FullName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Agrega un nuevo cliente a la base de datos
        /// </summary>
        public async Task AddAsync(Client client, CancellationToken ct)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            await _context.Clients.AddAsync(client, ct);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        public async Task UpdateAsync(Client client, CancellationToken ct)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            _context.Clients.Update(client);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Elimina un cliente por su ID
        /// </summary>
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (client is null)
                throw new InvalidOperationException($"Cliente con ID {id} no encontrado.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(ct);
        }
    }
}
