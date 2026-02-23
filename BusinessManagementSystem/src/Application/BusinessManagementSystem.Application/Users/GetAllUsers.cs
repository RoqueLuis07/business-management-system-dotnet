using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Users
{
    public static class GetAllUsers
    {
        public record Query;
        public record Result(
            Guid Id,
            string FullName,
            string Email,
            string Role,
            bool IsActive,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IUserRepository repo, CancellationToken ct)
        {
            var users = await repo.GetAllAsync(ct);
            return users.Select(MapToResult);
        }

        private static Result MapToResult(User user) =>
            new Result(user.Id, user.FullName, user.Email, user.Role.ToString(), user.IsActive, user.CreatedAtUtc);
    }
}
