using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;

        public UserRole Role { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public User(string fullName, string email, UserRole role, string? passwordHash = null)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(fullName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es obligatorio.", nameof(email));

            FullName = fullName.Trim();
            Email = email.Trim().ToLowerInvariant();
            Role = role;
            if (!string.IsNullOrWhiteSpace(passwordHash))
                PasswordHash = passwordHash;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public void ChangeRole(UserRole role) => Role = role;

        public void UpdateName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
        }
    }
}
