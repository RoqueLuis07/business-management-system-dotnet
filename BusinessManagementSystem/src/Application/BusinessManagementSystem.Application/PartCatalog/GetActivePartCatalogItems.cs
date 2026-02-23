using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.PartCatalog
{
    public static class GetActivePartCatalogItems
    {
        public record Query;
        public record Result(
            Guid Id,
            string Name,
            decimal DefaultUnitPrice);

        public static async Task<IEnumerable<Result>> HandleAsync(IPartCatalogRepository repo, CancellationToken ct)
        {
            var items = await repo.GetActiveAsync(ct);
            return items.Select(MapToResult);
        }

        private static Result MapToResult(PartCatalogItem item) =>
            new Result(item.Id, item.Name, item.DefaultUnitPrice);
    }
}
