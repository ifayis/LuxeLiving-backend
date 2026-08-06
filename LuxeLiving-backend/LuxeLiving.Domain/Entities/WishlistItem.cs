using LuxeLiving.Domain.Enitities;
using System.ComponentModel.DataAnnotations;

namespace LuxeLiving.Domain.Entities
{
    public class WishlistItem
    {
        public Guid Id { get; set; }

        public Guid WishlistId { get; set; }

        public Guid ProductId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }


        public Wishlist Wishlist { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}