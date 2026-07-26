using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Review
{
    public class UpdateReviewRequestDto
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;
    }
}