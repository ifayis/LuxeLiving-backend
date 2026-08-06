using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.DTOs.Checkout
{
    public class PaymentResponseDto
    {
        public Guid OrderId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}