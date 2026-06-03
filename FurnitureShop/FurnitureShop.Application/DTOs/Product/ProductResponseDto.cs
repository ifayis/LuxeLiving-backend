using System;

namespace FurnitureShop.Application.DTOs.Product
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public bool IsActive { get; set; }

        public int StockQuantity { get; set; }
    }
}
