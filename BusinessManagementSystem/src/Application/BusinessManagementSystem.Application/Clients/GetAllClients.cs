using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Clients
{
    public static class GetAllClients
    {
        public record Query;
        public record Result(
            Guid Id,
            string FullName,
            string Phone,
            string Address);

        public static async Task<IEnumerable<Result>> HandleAsync(IClientRepository repo, CancellationToken ct)
        {
            var clients = await repo.GetAllAsync(ct);
            return clients.Select(MapToResult);
        }

        private static Result MapToResult(Client client) =>
            new Result(client.Id, client.FullName, client.Phone, client.Address);
    }
}
