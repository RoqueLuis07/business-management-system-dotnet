using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;
using BusinessManagementSystem.Infrastructure.Data;

namespace BusinessManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de IUserRepository usando Entity Framework Core
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        /// <summary>
        /// Obtiene un usuario por su email (único)
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower(), ct);
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.FullName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene usuarios por rol específico
        /// </summary>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role, CancellationToken ct)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .AsNoTracking()
                .OrderBy(u => u.FullName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Obtiene solo usuarios activos
        /// </summary>
        public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct)
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .AsNoTracking()
                .OrderBy(u => u.FullName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Agrega un nuevo usuario a la base de datos
        /// </summary>
        public async Task AddAsync(User user, CancellationToken ct)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Elimina un usuario por su ID
        /// </summary>
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (user is null)
                throw new InvalidOperationException($"Usuario con ID {id} no encontrado.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
