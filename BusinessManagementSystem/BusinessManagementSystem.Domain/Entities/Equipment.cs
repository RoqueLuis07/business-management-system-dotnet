namespace BusinessManagementSystem.Domain.Entities
{
    public class Equipment
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Type { get; private set; }          // OBLIGATORIO
        public string Brand { get; private set; }         // OPCIONAL
        public string Model { get; private set; }         // OPCIONAL
        public string SerialNumber { get; private set; }  // OPCIONAL

        public bool IsIdentified =>
            !IsUnknown(Brand) || !IsUnknown(Model) || !IsUnknown(SerialNumber);

        public Equipment(
            string type,
            string? brand,
            string? model,
            string? serialNumber)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("El tipo de equipo es obligatorio.", nameof(type));

            Type = type.Trim();

            Brand = Normalize(brand);
            Model = Normalize(model);
            SerialNumber = Normalize(serialNumber);
        }

        public void MarkAsGenericChinese()
        {
            Brand = "Chino";
            Model = "Desconocido";
            SerialNumber = "Sin serie";
        }

        private static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Desconocido";

            return value.Trim();
        }

        private static bool IsUnknown(string value)
        {
            return value == "Desconocido" || value == "Sin serie";
        }
    }
}
