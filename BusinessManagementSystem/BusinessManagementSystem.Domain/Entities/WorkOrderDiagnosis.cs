namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrderDiagnosis
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Findings { get; private set; }          // Hallazgos
        public string RecommendedWork { get; private set; }   // Trabajo recomendado
        public string Notes { get; private set; }             // Observaciones extra

        public Guid MechanicUserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public WorkOrderDiagnosis(string findings, string recommendedWork, string? notes, Guid mechanicUserId)
        {
            if (string.IsNullOrWhiteSpace(findings))
                throw new ArgumentException("El diagnóstico (hallazgos) es obligatorio.", nameof(findings));

            if (string.IsNullOrWhiteSpace(recommendedWork))
                throw new ArgumentException("El trabajo recomendado es obligatorio.", nameof(recommendedWork));

            if (mechanicUserId == Guid.Empty)
                throw new ArgumentException("El mecánico no es válido.", nameof(mechanicUserId));

            Findings = findings.Trim();
            RecommendedWork = recommendedWork.Trim();
            Notes = string.IsNullOrWhiteSpace(notes) ? string.Empty : notes.Trim();
            MechanicUserId = mechanicUserId;
        }
    }
}
