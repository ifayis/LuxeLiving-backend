using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Product
{
    public class UpdateProductRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 9999999)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }
    }
}