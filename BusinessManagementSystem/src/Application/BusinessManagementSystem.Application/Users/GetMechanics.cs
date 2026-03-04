using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Application.Users
{
    public static class GetMechanics
    {
        public record Query;
        public record Result(
            Guid Id,
            string FullName,
            string Email,
            bool IsActive);

        public static async Task<IEnumerable<Result>> HandleAsync(IUserRepository repo, CancellationToken ct)
        {
            var mechanics = await repo.GetByRoleAsync(UserRole.Mecanico, ct);
            return mechanics.Select(MapToResult);
        }

        private static Result MapToResult(User user) =>
            new Result(user.Id, user.FullName, user.Email, user.IsActive);
    }
}
