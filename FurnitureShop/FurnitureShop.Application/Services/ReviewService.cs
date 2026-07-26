using FurnitureShop.Application.DTOs.Review;
using FurnitureShop.Application.Interfaces.Common;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(
            IReviewRepository reviewRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(
            Guid userId,
            CreateReviewRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException(
                    "Rating must be between 1 and 5.");
            }

            await ValidatePurchaseAsync(
                userId,
                request.OrderId,
                request.ProductId);

            var product =
                await ValidateProductAsync(
                    request.ProductId);

            var existingReview =
                await _reviewRepository.GetUserReviewAsync(
                    userId,
                    request.ProductId);

            if (existingReview != null)
            {
                throw new InvalidOperationException(
                    "You have already reviewed this product.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var review = new Review
                {
                    Id = Guid.NewGuid(),

                    ProductId = request.ProductId,

                    UserId = userId,

                    OrderId = request.OrderId,

                    Rating = request.Rating,

                    Comment = request.Comment.Trim(),

                    CreatedAt = DateTime.UtcNow
                };

                await _reviewRepository.AddAsync(review);

                await _reviewRepository
                    .RecalculateProductRatingAsync(
                        request.ProductId);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }

        public async Task UpdateAsync(
            Guid userId,
            Guid reviewId,
            UpdateReviewRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException(
                    "Rating must be between 1 and 5.");
            }

            var review =
                await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException(
                    "Review not found.");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only update your own review.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                review.Rating = request.Rating;

                review.Comment = request.Comment.Trim();

                review.UpdatedAt = DateTime.UtcNow;

                await _reviewRepository
                    .RecalculateProductRatingAsync(
                        review.ProductId);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }

        public async Task DeleteAsync(
            Guid userId,
            Guid reviewId)
        {
            var review =
                await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException(
                    "Review not found.");
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only delete your own review.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _reviewRepository.RemoveAsync(review);

                await _reviewRepository
                    .RecalculateProductRatingAsync(
                        review.ProductId);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }

        public async Task DeleteByAdminAsync(
            Guid reviewId)
        {
            var review =
                await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException(
                    "Review not found.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _reviewRepository.RemoveAsync(review);

                await _reviewRepository
                    .RecalculateProductRatingAsync(
                        review.ProductId);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }

        public async Task<ProductReviewSummaryDto>GetProductReviewsAsync(Guid productId)
        {
            var product =
                await _reviewRepository
                    .GetProductWithRatingAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException(
                    "Product not found.");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    "Product is unavailable.");
            }

            var reviews =
                await _reviewRepository
                    .GetProductReviewsAsync(productId);

            return new ProductReviewSummaryDto
            {
                AverageRating = product.AverageRating,

                ReviewCount = product.ReviewCount,

                Reviews = reviews
                    .Select(Map)
                    .ToList()
            };
        }

        private async Task ValidatePurchaseAsync(
            Guid userId,
            Guid orderId,
            Guid productId)
        {
            var purchased =
                await _orderRepository.HasPurchasedProductAsync(
                    userId,
                    orderId,
                    productId);

            if (!purchased)
            {
                throw new InvalidOperationException(
                    "You can review only products from your delivered orders.");
            }
        }

        private async Task<Product> ValidateProductAsync(
            Guid productId)
        {
            var product =
                await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException(
                    "Product not found.");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    "Product is unavailable.");
            }

            return product;
        }

        private static ReviewResponseDto Map(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                UserName = review.User.FullName,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };
        }

    }
}