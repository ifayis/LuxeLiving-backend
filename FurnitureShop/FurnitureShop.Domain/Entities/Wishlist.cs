namespace FurnitureShop.Domain.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}
