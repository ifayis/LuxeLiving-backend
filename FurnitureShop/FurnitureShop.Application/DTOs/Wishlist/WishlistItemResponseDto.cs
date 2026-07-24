namespace FurnitureShop.Application.DTOs.Wishlist
{
    public class WishlistItemResponseDto
    {
        public Guid WishlistItemId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public string? ImageUrl { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime AddedAt { get; set; }
    }
}