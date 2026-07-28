using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<Review?> GetByIdAsync(Guid reviewId)
        {
            return await _context.Reviews
                .Include(x => x.User)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == reviewId);
        }

        public async Task<Review?> GetUserReviewAsync(
            Guid userId,
            Guid productId)
        {
            return await _context.Reviews
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ProductId == productId
                );
        }

        public async Task<List<Review>> GetProductReviewsAsync(Guid productId)
        {
            return await _context.Reviews
                .Include(x => x.User)
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task RecalculateProductRatingAsync(Guid productId)
        {
            var product = await _context.Products
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return;
            }

            var reviews = product.Reviews.ToList();

            product.ReviewCount = reviews.Count;

            product.AverageRating =
                reviews.Count == 0
                    ? 0m
                    : Math.Round(
                        reviews.Average(x => (decimal)x.Rating),
                        1,
                        MidpointRounding.AwayFromZero
                    );

            product.UpdatedAt = DateTime.UtcNow;
        }

        public async Task<Product?> GetProductWithRatingAsync(Guid productId)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public Task RemoveAsync(Review review)
        {
            _context.Reviews.Remove(review);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}