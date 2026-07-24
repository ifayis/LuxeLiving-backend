using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class UpdateCartItemRequestDto
    {
        [Required(ErrorMessage = "Product is required.")]
        public Guid ProductId { get; set; }

        [Required]
        [Range(
            1,
            20,
            ErrorMessage = "Quantity must be between 1 and 20.")]
        public int Quantity { get; set; }
    }
}