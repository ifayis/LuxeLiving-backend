using FurnitureShop.Application.DTOs.Review;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task CreateAsync(
            Guid userId,
            CreateReviewRequestDto request);
        Task UpdateAsync(
            Guid userId,
            Guid reviewId,
            UpdateReviewRequestDto request);
        Task DeleteAsync(
            Guid userId,
            Guid reviewId);
        Task DeleteByAdminAsync(
            Guid reviewId);
        Task<ProductReviewSummaryDto>
            GetProductReviewsAsync(
                Guid productId);
    }
}