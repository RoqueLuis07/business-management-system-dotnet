using BusinessManagementSystem.Application.Abstractions;

namespace BusinessManagementSystem.Application.PartCatalog
{
    public static class DeletePartCatalogItem
    {
        public record Command(Guid ItemId);

        public static async Task HandleAsync(IPartCatalogRepository repo, Command cmd, CancellationToken ct)
        {
            var item = await repo.GetByIdAsync(cmd.ItemId, ct);
            if (item is null)
                throw new InvalidOperationException("No se encontró el repuesto en el catálogo.");

            await repo.DeleteAsync(cmd.ItemId, ct);
        }
    }
}
