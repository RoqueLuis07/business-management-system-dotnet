using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.Clients
{
    public static class DeleteClient
    {
        public record Command(Guid ClientId);

        public static async Task HandleAsync(IClientRepository repo, Command cmd, CancellationToken ct)
        {
            var client = await repo.GetByIdAsync(cmd.ClientId, ct);
            if (client is null)
                throw new InvalidOperationException("No se encontró el cliente.");

            await repo.DeleteAsync(cmd.ClientId, ct);
        }
    }
}
