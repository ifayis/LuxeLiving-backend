using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Review
{
    public class CreateReviewRequestDto
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;
    }
}