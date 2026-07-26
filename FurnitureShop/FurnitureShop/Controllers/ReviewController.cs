using FurnitureShop.Application.common;
using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Review;
using FurnitureShop.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurnitureShop.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(
            IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirstValue("UID");

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    ErrorMessages.InvalidToken);
            }

            return Guid.Parse(userId);
        }

        #region Customer

        [Authorize(Roles = Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateReviewRequestDto request)
        {
            await _reviewService.CreateAsync(
                GetUserId(),
                request);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "Review added successfully."));
        }

        [Authorize(Roles = Roles.User)]
        [HttpPut("{reviewId:guid}")]
        public async Task<IActionResult> Update(
            Guid reviewId,
            UpdateReviewRequestDto request)
        {
            await _reviewService.UpdateAsync(
                GetUserId(),
                reviewId,
                request);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "Review updated successfully."));
        }

        [Authorize(Roles = Roles.User)]
        [HttpDelete("{reviewId:guid}")]
        public async Task<IActionResult> Delete(
            Guid reviewId)
        {
            await _reviewService.DeleteAsync(
                GetUserId(),
                reviewId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "Review deleted successfully."));
        }

        #endregion

        #region Public

        [AllowAnonymous]
        [HttpGet("product/{productId:guid}")]
        public async Task<IActionResult> GetProductReviews(
            Guid productId)
        {
            var reviews =
                await _reviewService
                    .GetProductReviewsAsync(productId);

            return Ok(reviews);
        }

        #endregion

        #region Admin

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("admin/{reviewId:guid}")]
        public async Task<IActionResult> DeleteByAdmin(
            Guid reviewId)
        {
            await _reviewService.DeleteByAdminAsync(
                reviewId);

            return Ok(
                ApiResponse<object>.Success(
                    null,
                    "Review deleted successfully."));
        }

        #endregion
    }
}