using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using FurnitureShop.Domain.Enums;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ShippingAddressId { get; set; }

    public ShippingAddress ShippingAddress { get; set; } = null!;

    public PaymentMethod PaymentMethod { get; set; }

    public OrderStatus Status { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();
}