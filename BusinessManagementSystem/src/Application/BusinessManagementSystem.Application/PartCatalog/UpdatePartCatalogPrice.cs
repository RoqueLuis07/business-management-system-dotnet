using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.PartCatalog
{
    public static class UpdatePartCatalogPrice
    {
        public record Command(Guid ItemId, decimal NewPrice);

        public static async Task HandleAsync(IPartCatalogRepository repo, Command cmd, CancellationToken ct)
        {
            var item = await repo.GetByIdAsync(cmd.ItemId, ct);
            if (item is null)
                throw new InvalidOperationException("No se encontró el repuesto en el catálogo.");

            item.UpdatePrice(cmd.NewPrice);

            await repo.UpdateAsync(item, ct);
        }
    }
}
