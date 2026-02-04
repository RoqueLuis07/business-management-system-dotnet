namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrderAccessory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; }
        public bool IsPresent { get; private set; }
        public string Condition { get; private set; }

        public DateTime RegisteredAtUtc { get; private set; } = DateTime.UtcNow;

        public WorkOrderAccessory(string name, bool isPresent, string? condition)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del accesorio es obligatorio.", nameof(name));

            Name = name.Trim();
            IsPresent = isPresent;

            if (!IsPresent && string.IsNullOrWhiteSpace(condition))
                Condition = "No trae";
            else
                Condition = string.IsNullOrWhiteSpace(condition) ? "Sin observaciones" : condition.Trim();
        }
    }
}
