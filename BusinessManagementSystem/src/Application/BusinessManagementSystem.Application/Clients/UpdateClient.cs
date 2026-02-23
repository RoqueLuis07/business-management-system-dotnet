using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.Clients
{
    public static class UpdateClient
    {
        public record Command(
            Guid ClientId,
            string FullName,
            string Phone,
            string Address);

        public static async Task HandleAsync(IClientRepository repo, Command cmd, CancellationToken ct)
        {
            var client = await repo.GetByIdAsync(cmd.ClientId, ct);
            if (client is null)
                throw new InvalidOperationException("No se encontró el cliente.");

            // Validación: si cambió el teléfono, verificar que no exista otro cliente con ese teléfono
            if (client.Phone != cmd.Phone && !string.IsNullOrWhiteSpace(cmd.Phone))
            {
                var existingWithPhone = await repo.GetByPhoneAsync(cmd.Phone.Trim(), ct);
                if (existingWithPhone is not null && existingWithPhone.Id != cmd.ClientId)
                    throw new InvalidOperationException("Ya existe otro cliente con ese teléfono.");
            }

            client.UpdateInfo(cmd.FullName, cmd.Phone, cmd.Address);

            await repo.UpdateAsync(client, ct);
        }
    }
}
