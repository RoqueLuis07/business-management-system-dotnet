namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrderServiceReport
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string WorkPerformed { get; private set; }     // qué se hizo
        public string Recommendations { get; private set; }   // recomendaciones / advertencias
        public string Notes { get; private set; }             // extras

        public Guid MechanicUserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public WorkOrderServiceReport(string workPerformed, string? recommendations, string? notes, Guid mechanicUserId)
        {
            if (string.IsNullOrWhiteSpace(workPerformed))
                throw new ArgumentException("El trabajo realizado es obligatorio.", nameof(workPerformed));

            if (mechanicUserId == Guid.Empty)
                throw new ArgumentException("El mecánico no es válido.", nameof(mechanicUserId));

            WorkPerformed = workPerformed.Trim();
            Recommendations = string.IsNullOrWhiteSpace(recommendations) ? string.Empty : recommendations.Trim();
            Notes = string.IsNullOrWhiteSpace(notes) ? string.Empty : notes.Trim();
            MechanicUserId = mechanicUserId;
        }
    }
}
