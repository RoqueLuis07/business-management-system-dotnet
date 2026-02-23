using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Clients
{
    public static class CreateClient
    {
        public record Command(
            string FullName,
            string Phone,
            string Address);

        public static async Task<Guid> HandleAsync(IClientRepository repo, Command cmd, CancellationToken ct)
        {
            // Validación: teléfono único (regla de negocio)
            if (!string.IsNullOrWhiteSpace(cmd.Phone))
            {
                var existing = await repo.GetByPhoneAsync(cmd.Phone.Trim(), ct);
                if (existing is not null)
                    throw new InvalidOperationException("Ya existe un cliente con ese teléfono.");
            }

            var client = new Client(cmd.FullName, cmd.Phone, cmd.Address);

            await repo.AddAsync(client, ct);
            return client.Id;
        }
    }
}
