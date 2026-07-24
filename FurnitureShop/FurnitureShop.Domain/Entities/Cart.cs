using FurnitureShop.Domain.Enitities;
using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastActivityAt { get; set; } = DateTime.UtcNow;

        public bool IsCheckedOut { get; set; } = false;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}