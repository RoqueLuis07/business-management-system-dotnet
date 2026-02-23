using BusinessManagementSystem.Application.Abstractions;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Application.WarrantyClaims
{
    public static class GetWarrantyClaimById
    {
        public record Query(Guid ClaimId);
        public record Result(
            Guid Id,
            Guid OriginalWorkOrderId,
            Guid ClaimWorkOrderId,
            string Reason,
            Guid CreatedByUserId,
            DateTime CreatedAtUtc);

        public static async Task<Result> HandleAsync(IWarrantyClaimRepository repo, Query query, CancellationToken ct)
        {
            var claim = await repo.GetByIdAsync(query.ClaimId, ct);
            if (claim is null)
                throw new InvalidOperationException("No se encontró el reclamo de garantía.");

            return MapToResult(claim);
        }

        private static Result MapToResult(WarrantyClaim claim) =>
            new Result(claim.Id, claim.OriginalWorkOrderId, claim.ClaimWorkOrderId, claim.Reason, claim.CreatedByUserId, claim.CreatedAtUtc);
    }
}
