using FurnitureShop.Domain.Entities;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Domain.Enitities
{
    public class Order
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid ShippingAddressId { get; set; }

        public ShippingAddress ShippingAddress { get; set; } = null!;

        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus Status { get; set; }
            = OrderStatus.Pending;

        public decimal SubTotal { get; set; }

        public decimal ShippingCharge { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();
    }
}