using System.ComponentModel.DataAnnotations;
using LuxeLiving.Domain.Entities;

namespace LuxeLiving.Domain.Enitities
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(180)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(3000)]
        public string Description { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsBestSeller { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public decimal AverageRating { get; set; }

        public int ReviewCount { get; set; }

        public Category Category { get; set; } = null!;


        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}