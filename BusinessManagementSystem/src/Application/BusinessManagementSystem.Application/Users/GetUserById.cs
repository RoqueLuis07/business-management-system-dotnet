using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.Users
{
    public static class GetUserById
    {
        public record Query(Guid UserId);
        public record Result(
            Guid Id,
            string FullName,
            string Email,
            string Role,
            bool IsActive,
            DateTime CreatedAtUtc);

        public static async Task<Result> HandleAsync(IUserRepository repo, Query query, CancellationToken ct)
        {
            var user = await repo.GetByIdAsync(query.UserId, ct);
            if (user is null)
                throw new InvalidOperationException("No se encontró el usuario.");

            return MapToResult(user);
        }

        private static Result MapToResult(User user) =>
            new Result(user.Id, user.FullName, user.Email, user.Role.ToString(), user.IsActive, user.CreatedAtUtc);
    }
}
