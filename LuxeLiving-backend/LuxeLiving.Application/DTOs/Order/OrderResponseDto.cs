using LuxeLiving.Application.DTOs.ShippingAddress;
using LuxeLiving.Domain.Enums;

namespace LuxeLiving.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ShippingCharge { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public string? CancellationReason { get; set; }

        public DateTime? CancelledAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public ShippingAddressResponseDto ShippingAddress { get; set; } = null!;

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}