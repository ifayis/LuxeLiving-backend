namespace FurnitureShop.Application.DTOs.Wishlist
{
    public class WishlistItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
