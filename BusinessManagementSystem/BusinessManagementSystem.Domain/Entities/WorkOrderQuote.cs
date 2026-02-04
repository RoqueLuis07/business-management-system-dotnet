namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrderQuote
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public decimal LaborCost { get; private set; }
        public decimal PartsTotal { get; private set; }
        public decimal Total => LaborCost + PartsTotal;

        public string Notes { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public WorkOrderQuote(decimal laborCost, decimal partsTotal, string? notes, Guid createdByUserId)
        {
            if (laborCost < 0)
                throw new ArgumentOutOfRangeException(nameof(laborCost), "La mano de obra no puede ser negativa.");

            if (partsTotal < 0)
                throw new ArgumentOutOfRangeException(nameof(partsTotal), "El total de repuestos no puede ser negativo.");

            if (createdByUserId == Guid.Empty)
                throw new ArgumentException("El usuario creador no es válido.", nameof(createdByUserId));

            LaborCost = laborCost;
            PartsTotal = partsTotal;
            Notes = string.IsNullOrWhiteSpace(notes) ? string.Empty : notes.Trim();
            CreatedByUserId = createdByUserId;
        }

        public void Update(decimal laborCost, decimal partsTotal, string? notes)
        {
            if (laborCost < 0)
                throw new ArgumentOutOfRangeException(nameof(laborCost), "La mano de obra no puede ser negativa.");

            if (partsTotal < 0)
                throw new ArgumentOutOfRangeException(nameof(partsTotal), "El total de repuestos no puede ser negativo.");

            LaborCost = laborCost;
            PartsTotal = partsTotal;
            Notes = string.IsNullOrWhiteSpace(notes) ? string.Empty : notes.Trim();
        }
    }
}
