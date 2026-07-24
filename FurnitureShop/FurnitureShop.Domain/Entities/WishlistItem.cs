using FurnitureShop.Domain.Enitities;
using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Domain.Entities
{
    public class WishlistItem
    {
        public Guid Id { get; set; }

        public Guid WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = null!;

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}