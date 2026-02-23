namespace BusinessManagementSystem.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string FullName { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }

        public Client(string fullName, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
            Phone = phone?.Trim() ?? string.Empty;
            Address = address?.Trim() ?? string.Empty;
        }

        public void UpdateInfo(string fullName, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
            Phone = phone?.Trim() ?? string.Empty;
            Address = address?.Trim() ?? string.Empty;
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
