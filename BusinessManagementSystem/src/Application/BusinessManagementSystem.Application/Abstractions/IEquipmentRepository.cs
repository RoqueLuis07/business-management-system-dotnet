using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Abstractions
{
    /// <summary>
    /// Repositorio para gestionar equipos (Equipment) en el dominio.
    /// </summary>
    public interface IEquipmentRepository
    {
        Task<Equipment?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Equipment?> GetBySerialNumberAsync(string serialNumber, CancellationToken ct);
        Task<IEnumerable<Equipment>> GetAllAsync(CancellationToken ct);

        Task AddAsync(Equipment equipment, CancellationToken ct);
        Task UpdateAsync(Equipment equipment, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}