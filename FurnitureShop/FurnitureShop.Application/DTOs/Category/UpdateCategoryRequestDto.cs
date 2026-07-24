using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Category
{
    public class UpdateCategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(
            50,
            MinimumLength = 2,
            ErrorMessage = "Category name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(
            500,
            ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Invalid image URL.")]
        [StringLength(
            500,
            ErrorMessage = "Image URL cannot exceed 500 characters.")]
        public string? ImageUrl { get; set; }

        [Range(
            0,
            999,
            ErrorMessage = "Display order must be between 0 and 999.")]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}