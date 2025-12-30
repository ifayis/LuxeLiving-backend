namespace FurnitureShop.Application.DTOs.Wishlist
{
    public class WishlistResponseDto
    {
        public Guid Id { get; set; }
        public List<WishlistItemResponseDto> Items { get; set; }
    }
}
