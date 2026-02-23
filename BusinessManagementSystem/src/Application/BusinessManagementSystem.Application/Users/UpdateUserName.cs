using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.Users
{
    public static class UpdateUserName
    {
        public record Command(Guid UserId, string FullName);

        public static async Task HandleAsync(IUserRepository repo, Command cmd, CancellationToken ct)
        {
            var user = await repo.GetByIdAsync(cmd.UserId, ct);
            if (user is null)
                throw new InvalidOperationException("No se encontró el usuario.");

            user.UpdateName(cmd.FullName);

            await repo.UpdateAsync(user, ct);
        }
    }
}
