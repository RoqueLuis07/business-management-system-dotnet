namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrderPart
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Lo que el mecánico declara (texto libre, realista)
        public string PartName { get; private set; }

        public int Quantity { get; private set; }

        // Se completa después (por admin) según inventario/mercado
        public decimal? UnitPrice { get; private set; }

        // Vinculación opcional al catálogo (si se logra identificar)
        public Guid? CatalogItemId { get; private set; }

        public DateTime RegisteredAtUtc { get; private set; } = DateTime.UtcNow;

        public decimal? LineTotal => UnitPrice is null ? null : UnitPrice.Value * Quantity;

        public WorkOrderPart(string partName, int quantity)
        {
            if (string.IsNullOrWhiteSpace(partName))
                throw new ArgumentException("El nombre del repuesto es obligatorio.", nameof(partName));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor a cero.");

            PartName = partName.Trim();
            Quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor a cero.");

            Quantity = quantity;
        }

        // Administración “precio” la línea (puede ser desde catálogo o manual)
        public void SetPricing(decimal unitPrice, Guid? catalogItemId = null)
        {
            if (unitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "El precio no puede ser negativo.");

            UnitPrice = unitPrice;
            CatalogItemId = catalogItemId;
        }

        public void ClearPricing()
        {
            UnitPrice = null;
            CatalogItemId = null;
        }
    }
}
