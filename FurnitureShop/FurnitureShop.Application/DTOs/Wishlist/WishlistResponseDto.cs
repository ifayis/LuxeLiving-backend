namespace FurnitureShop.Application.DTOs.Wishlist
{
    public class WishlistResponseDto
    {
        public Guid WishlistId { get; set; }

        public int TotalItems { get; set; }

        public List<WishlistItemResponseDto> Items { get; set; }
            = new();
    }
}