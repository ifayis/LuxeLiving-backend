namespace FurnitureShop.Application.DTOs.Review
{
    public class ProductReviewSummaryDto
    {
        public decimal AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public List<ReviewResponseDto> Reviews { get; set; }
            = new();
    }
}