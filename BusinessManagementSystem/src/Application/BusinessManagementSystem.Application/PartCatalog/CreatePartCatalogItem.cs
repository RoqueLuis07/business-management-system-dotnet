using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.PartCatalog
{
    public static class CreatePartCatalogItem
    {
        public record Command(
            string Name,
            decimal DefaultUnitPrice);

        public static async Task<Guid> HandleAsync(IPartCatalogRepository repo, Command cmd, CancellationToken ct)
        {
            // Validación: nombre único
            var existing = await repo.GetByNameAsync(cmd.Name.Trim(), ct);
            if (existing is not null)
                throw new InvalidOperationException("Ya existe un repuesto con ese nombre en el catálogo.");

            var item = new PartCatalogItem(cmd.Name, cmd.DefaultUnitPrice);

            await repo.AddAsync(item, ct);
            return item.Id;
        }
    }
}
