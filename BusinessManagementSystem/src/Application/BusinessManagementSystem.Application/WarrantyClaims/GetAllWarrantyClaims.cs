using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.WarrantyClaims
{
    public static class GetAllWarrantyClaims
    {
        public record Query;
        public record Result(
            Guid Id,
            Guid OriginalWorkOrderId,
            Guid ClaimWorkOrderId,
            string Reason,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IWarrantyClaimRepository repo, CancellationToken ct)
        {
            var claims = await repo.GetAllAsync(ct);
            return claims.Select(MapToResult);
        }

        private static Result MapToResult(WarrantyClaim claim) =>
            new Result(claim.Id, claim.OriginalWorkOrderId, claim.ClaimWorkOrderId, claim.Reason, claim.CreatedAtUtc);
    }
}
