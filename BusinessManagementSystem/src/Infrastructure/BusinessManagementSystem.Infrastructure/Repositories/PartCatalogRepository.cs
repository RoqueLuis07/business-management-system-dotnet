using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Infrastructure.Data;

namespace BusinessManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IPartCatalogRepository usando Entity Framework Core
    /// </summary>
    public class PartCatalogRepository : IPartCatalogRepository
    {
        private readonly ApplicationDbContext _context;

        public PartCatalogRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene un repuesto por su ID
        /// </summary>
        public async Task<PartCatalogItem?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.PartCatalogItems
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        /// <summary>
        /// Obtiene un repuesto por su nombre (único)
        /// </summary>
        public async Task<PartCatalogItem?> GetByNameAsync(string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await _context.PartCatalogItems
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name.Trim(), ct);
        }

        /// <summary>
        /// Obtiene todos los repuestos
        /// </summary>
        public async Task<IEnumerable<PartCatalogItem>> GetAllAsync(CancellationToken ct)
        {
            return await _context.PartCatalogItems
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene solo repuestos activos
        /// </summary>
        public async Task<IEnumerable<PartCatalogItem>> GetActiveAsync(CancellationToken ct)
        {
            return await _context.PartCatalogItems
                .Where(p => p.IsActive)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Agrega un nuevo repuesto al catálogo
        /// </summary>
        public async Task AddAsync(PartCatalogItem item, CancellationToken ct)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            await _context.PartCatalogItems.AddAsync(item, ct);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Actualiza un repuesto existente
        /// </summary>
        public async Task UpdateAsync(PartCatalogItem item, CancellationToken ct)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _context.PartCatalogItems.Update(item);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Elimina un repuesto del catálogo
        /// </summary>
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var item = await _context.PartCatalogItems.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (item is null)
                throw new InvalidOperationException($"Repuesto con ID {id} no encontrado.");

            _context.PartCatalogItems.Remove(item);
            await _context.SaveChangesAsync(ct);
        }
    }
}
