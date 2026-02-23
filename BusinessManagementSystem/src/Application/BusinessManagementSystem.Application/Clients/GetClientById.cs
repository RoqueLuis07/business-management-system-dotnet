using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Clients
{
    public static class GetClientById
    {
        public record Query(Guid ClientId);
        public record Result(
            Guid Id,
            string FullName,
            string Phone,
            string Address);

        public static async Task<Result> HandleAsync(IClientRepository repo, Query query, CancellationToken ct)
        {
            var client = await repo.GetByIdAsync(query.ClientId, ct);
            if (client is null)
                throw new InvalidOperationException("No se encontró el cliente.");

            return MapToResult(client);
        }

        private static Result MapToResult(Client client) =>
            new Result(client.Id, client.FullName, client.Phone, client.Address);
    }
}
