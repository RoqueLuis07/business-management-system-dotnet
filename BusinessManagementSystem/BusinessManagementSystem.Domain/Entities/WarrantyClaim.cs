namespace BusinessManagementSystem.Domain.Entities
{
    public class WarrantyClaim
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // OT original (la que generó la garantía)
        public Guid OriginalWorkOrderId { get; private set; }

        // OT nueva (la de reingreso por garantía)
        public Guid ClaimWorkOrderId { get; private set; }

        public string Reason { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public WarrantyClaim(Guid originalWorkOrderId, Guid claimWorkOrderId, string reason, Guid createdByUserId)
        {
            if (originalWorkOrderId == Guid.Empty)
                throw new ArgumentException("La OT original no es válida.", nameof(originalWorkOrderId));

            if (claimWorkOrderId == Guid.Empty)
                throw new ArgumentException("La OT de garantía no es válida.", nameof(claimWorkOrderId));

            if (originalWorkOrderId == claimWorkOrderId)
                throw new ArgumentException("La OT original y la OT de garantía no pueden ser la misma.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("El motivo de la garantía es obligatorio.", nameof(reason));

            if (createdByUserId == Guid.Empty)
                throw new ArgumentException("El usuario creador no es válido.", nameof(createdByUserId));

            OriginalWorkOrderId = originalWorkOrderId;
            ClaimWorkOrderId = claimWorkOrderId;
            Reason = reason.Trim();
            CreatedByUserId = createdByUserId;
        }
    }
}
