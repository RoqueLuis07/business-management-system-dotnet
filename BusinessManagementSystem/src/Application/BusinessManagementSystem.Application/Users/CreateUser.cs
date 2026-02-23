using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Application.Users
{
    public static class CreateUser
    {
        public record Command(
            string FullName,
            string Email,
            UserRole Role);

        public static async Task<Guid> HandleAsync(IUserRepository repo, Command cmd, CancellationToken ct)
        {
            // Validación: email único (regla de negocio)
            var existing = await repo.GetByEmailAsync(cmd.Email.Trim().ToLowerInvariant(), ct);
            if (existing is not null)
                throw new InvalidOperationException("Ya existe un usuario con ese email.");

            var user = new User(cmd.FullName, cmd.Email, cmd.Role);

            await repo.AddAsync(user, ct);
            return user.Id;
        }
    }
}
