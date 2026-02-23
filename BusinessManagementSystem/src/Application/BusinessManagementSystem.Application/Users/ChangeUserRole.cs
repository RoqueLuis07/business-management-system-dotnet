using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Application.Users
{
    public static class ChangeUserRole
    {
        public record Command(Guid UserId, UserRole NewRole);

        public static async Task HandleAsync(IUserRepository repo, Command cmd, CancellationToken ct)
        {
            var user = await repo.GetByIdAsync(cmd.UserId, ct);
            if (user is null)
                throw new InvalidOperationException("No se encontró el usuario.");

            user.ChangeRole(cmd.NewRole);

            await repo.UpdateAsync(user, ct);
        }
    }
}
