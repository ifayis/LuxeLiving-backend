using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Wishlist
{
    public class AddToWishlistRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
