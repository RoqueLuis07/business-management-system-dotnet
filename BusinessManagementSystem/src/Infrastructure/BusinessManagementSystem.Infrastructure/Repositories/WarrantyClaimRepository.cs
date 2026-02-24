using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Infrastructure.Data;

namespace BusinessManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IWarrantyClaimRepository usando Entity Framework Core
    /// </summary>
    public class WarrantyClaimRepository : IWarrantyClaimRepository
    {
        private readonly ApplicationDbContext _context;

        public WarrantyClaimRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene un reclamo de garantía por su ID
        /// </summary>
        public async Task<WarrantyClaim?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.WarrantyClaims
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id, ct);
        }

        /// <summary>
        /// Obtiene todos los reclamos de garantía de una orden original
        /// </summary>
        public async Task<IEnumerable<WarrantyClaim>> GetByOriginalWorkOrderAsync(Guid originalWorkOrderId, CancellationToken ct)
        {
            return await _context.WarrantyClaims
                .Where(w => w.OriginalWorkOrderId == originalWorkOrderId)
                .AsNoTracking()
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene todos los reclamos de garantía de una orden de reclamación
        /// </summary>
        public async Task<IEnumerable<WarrantyClaim>> GetByClaimWorkOrderAsync(Guid claimWorkOrderId, CancellationToken ct)
        {
            return await _context.WarrantyClaims
                .Where(w => w.ClaimWorkOrderId == claimWorkOrderId)
                .AsNoTracking()
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene todos los reclamos de garantía
        /// </summary>
        public async Task<IEnumerable<WarrantyClaim>> GetAllAsync(CancellationToken ct)
        {
            return await _context.WarrantyClaims
                .AsNoTracking()
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Agrega un nuevo reclamo de garantía
        /// </summary>
        public async Task AddAsync(WarrantyClaim claim, CancellationToken ct)
        {
            if (claim is null)
                throw new ArgumentNullException(nameof(claim));

            await _context.WarrantyClaims.AddAsync(claim, ct);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Actualiza un reclamo de garantía existente
        /// </summary>
        public async Task UpdateAsync(WarrantyClaim claim, CancellationToken ct)
        {
            if (claim is null)
                throw new ArgumentNullException(nameof(claim));

            _context.WarrantyClaims.Update(claim);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Elimina un reclamo de garantía
        /// </summary>
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var claim = await _context.WarrantyClaims.FirstOrDefaultAsync(w => w.Id == id, ct);
            if (claim is null)
                throw new InvalidOperationException($"Reclamo de garantía con ID {id} no encontrado.");

            _context.WarrantyClaims.Remove(claim);
            await _context.SaveChangesAsync(ct);
        }
    }
}
