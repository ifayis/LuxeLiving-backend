using System.ComponentModel.DataAnnotations;

namespace LuxeLiving.Application.DTOs.Wishlist
{
    public class AddToWishlistRequestDto
    {
        [Required(ErrorMessage = "Product is required.")]
        public Guid ProductId { get; set; }
    }
}