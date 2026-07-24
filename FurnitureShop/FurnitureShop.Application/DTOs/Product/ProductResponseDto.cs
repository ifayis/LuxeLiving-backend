namespace FurnitureShop.Application.DTOs.Product
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int StockQuantity { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsBestSeller { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}