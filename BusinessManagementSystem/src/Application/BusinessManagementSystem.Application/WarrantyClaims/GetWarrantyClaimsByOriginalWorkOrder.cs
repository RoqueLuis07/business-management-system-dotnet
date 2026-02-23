using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.WarrantyClaims
{
    public static class GetWarrantyClaimsByOriginalWorkOrder
    {
        public record Query(Guid OriginalWorkOrderId);
        public record Result(
            Guid Id,
            Guid ClaimWorkOrderId,
            string Reason,
            DateTime CreatedAtUtc);

        public static async Task<IEnumerable<Result>> HandleAsync(IWarrantyClaimRepository repo, Query query, CancellationToken ct)
        {
            var claims = await repo.GetByOriginalWorkOrderAsync(query.OriginalWorkOrderId, ct);
            return claims.Select(MapToResult);
        }

        private static Result MapToResult(WarrantyClaim claim) =>
            new Result(claim.Id, claim.ClaimWorkOrderId, claim.Reason, claim.CreatedAtUtc);
    }
}
