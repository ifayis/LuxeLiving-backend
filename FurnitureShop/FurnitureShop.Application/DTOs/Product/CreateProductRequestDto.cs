using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Product
{
    public class CreateProductRequestDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(
            150,
            MinimumLength = 2,
            ErrorMessage = "Product name must be between 2 and 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(
            3000,
            ErrorMessage = "Description cannot exceed 3000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 99999999)]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Range(0.01, 99999999)]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Url]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsBestSeller { get; set; }
    }
}