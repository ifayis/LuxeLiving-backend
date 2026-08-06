using LuxeLiving.Domain.Enitities;
using LuxeLiving.Domain.Entities;

namespace LuxeLiving.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<Review?> GetByIdAsync(Guid reviewId);
        Task<Review?> GetUserReviewAsync(Guid userId, Guid productId);
        Task RecalculateProductRatingAsync(Guid productId);
        Task<List<Review>> GetProductReviewsAsync(Guid productId);
        Task RemoveAsync(Review review);
        Task<Product?> GetProductWithRatingAsync(Guid productId);
        Task SaveChangesAsync();
    }
}