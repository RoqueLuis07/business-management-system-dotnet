using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;
using BusinessManagementSystem.Infrastructure.Data;

namespace BusinessManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IWorkOrderRepository usando Entity Framework Core
    /// </summary>
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkOrderRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene una orden de trabajo por su ID (con todas sus relaciones)
        /// </summary>
        public async Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.WorkOrders
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .FirstOrDefaultAsync(w => w.Id == id, ct);
        }

        /// <summary>
        /// Obtiene una orden de trabajo por su número (único)
        /// </summary>
        public async Task<WorkOrder?> GetByNumberAsync(string workOrderNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(workOrderNumber))
                return null;

            return await _context.WorkOrders
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .FirstOrDefaultAsync(w => w.WorkOrderNumber == workOrderNumber.Trim(), ct);
        }

        /// <summary>
        /// Obtiene todas las órdenes de trabajo
        /// </summary>
        public async Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken ct)
        {
            return await _context.WorkOrders
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene órdenes por estado
        /// </summary>
        public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct)
        {
            return await _context.WorkOrders
                .Where(w => w.Status == status)
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene órdenes de trabajo de un cliente específico
        /// </summary>
        public async Task<IEnumerable<WorkOrder>> GetByClientAsync(Guid clientId, CancellationToken ct)
        {
            return await _context.WorkOrders
                .Where(w => w.Client.Id == clientId)
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene órdenes de trabajo asignadas a un mecánico
        /// </summary>
        public async Task<IEnumerable<WorkOrder>> GetByMechanicAsync(Guid mechanicUserId, CancellationToken ct)
        {
            return await _context.WorkOrders
                .Where(w => w.AssignedMechanicUserId == mechanicUserId)
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .OrderByDescending(w => w.CreatedAtUtc)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene órdenes que están bajo garantía (comparando con fecha actual local)
        /// </summary>
        public async Task<IEnumerable<WorkOrder>> GetUnderWarrantyAsync(DateTime nowLocal, CancellationToken ct)
        {
            // Órdenes entregadas
            var deliveredOrders = await _context.WorkOrders
                .Where(w => w.Status == WorkOrderStatus.Entregada && w.DeliveredAtLocal.HasValue)
                .Include(w => w.Client)
                .Include(w => w.Equipment)
                .ToListAsync(ct);

            // Filtrar por garantía vigente
            return deliveredOrders
                .Where(w => w.IsUnderWarranty(nowLocal))
                .OrderByDescending(w => w.DeliveredAtLocal)
                .ToList();
        }

        /// <summary>
        /// Agrega una nueva orden de trabajo a la base de datos
        /// </summary>
        public async Task AddAsync(WorkOrder workOrder, CancellationToken ct)
        {
            if (workOrder is null)
                throw new ArgumentNullException(nameof(workOrder));

            await _context.WorkOrders.AddAsync(workOrder, ct);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Actualiza una orden de trabajo existente
        /// </summary>
        public async Task UpdateAsync(WorkOrder workOrder, CancellationToken ct)
        {
            if (workOrder is null)
                throw new ArgumentNullException(nameof(workOrder));

            _context.WorkOrders.Update(workOrder);
            await _context.SaveChangesAsync(ct);
        }
    }
}
