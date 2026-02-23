using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.Users
{
    public static class DeactivateUser
    {
        public record Command(Guid UserId);

        public static async Task HandleAsync(IUserRepository repo, Command cmd, CancellationToken ct)
        {
            var user = await repo.GetByIdAsync(cmd.UserId, ct);
            if (user is null)
                throw new InvalidOperationException("No se encontró el usuario.");

            user.Deactivate();

            await repo.UpdateAsync(user, ct);
        }
    }
}
