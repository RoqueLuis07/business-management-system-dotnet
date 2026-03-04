namespace BusinessManagementSystem.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string Observations { get; private set; } = string.Empty;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public Client(string fullName, string phone, string address, string? email = null, string? observations = null)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
            Phone = phone?.Trim() ?? string.Empty;
            Address = address?.Trim() ?? string.Empty;
            Email = email?.Trim().ToLowerInvariant() ?? string.Empty;
            Observations = observations?.Trim() ?? string.Empty;
        }

        public void UpdateInfo(string fullName, string phone, string address, string? email = null, string? observations = null)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
            Phone = phone?.Trim() ?? string.Empty;
            Address = address?.Trim() ?? string.Empty;
            Email = email?.Trim().ToLowerInvariant() ?? Email;
            Observations = observations?.Trim() ?? Observations;
        }

        public void UpdatePhone(string phone)
        {
            Phone = phone?.Trim() ?? string.Empty;
        }

        public void UpdateAddress(string address)
        {
            Address = address?.Trim() ?? string.Empty;
        }
    }
}
