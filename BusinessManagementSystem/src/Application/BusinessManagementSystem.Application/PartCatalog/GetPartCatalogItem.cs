using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.PartCatalog
{
    public static class GetPartCatalogItem
    {
        public record Query(Guid ItemId);
        public record Result(
            Guid Id,
            string Name,
            decimal DefaultUnitPrice,
            bool IsActive);

        public static async Task<Result> HandleAsync(IPartCatalogRepository repo, Query query, CancellationToken ct)
        {
            var item = await repo.GetByIdAsync(query.ItemId, ct);
            if (item is null)
                throw new InvalidOperationException("No se encontró el repuesto en el catálogo.");

            return MapToResult(item);
        }

        private static Result MapToResult(PartCatalogItem item) =>
            new Result(item.Id, item.Name, item.DefaultUnitPrice, item.IsActive);
    }
}
