using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 9999999)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}