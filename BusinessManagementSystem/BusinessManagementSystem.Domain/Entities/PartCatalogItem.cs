namespace BusinessManagementSystem.Domain.Entities
{
    public class PartCatalogItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; }
        public decimal DefaultUnitPrice { get; private set; }

        public bool IsActive { get; private set; } = true;

        public PartCatalogItem(string name, decimal defaultUnitPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del repuesto es obligatorio.", nameof(name));
            if (defaultUnitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(defaultUnitPrice), "El precio no puede ser negativo.");

            Name = name.Trim();
            DefaultUnitPrice = defaultUnitPrice;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "El precio no puede ser negativo.");

            DefaultUnitPrice = newPrice;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
