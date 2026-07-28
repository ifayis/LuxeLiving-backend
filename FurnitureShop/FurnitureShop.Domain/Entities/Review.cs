using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Product Product { get; set; } = null!;

        public User User { get; set; } = null!;

        public Order Order { get; set; } = null!;
    }
}