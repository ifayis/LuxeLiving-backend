using FurnitureShop.Application.DTOs.ShippingAddress;

namespace FurnitureShop.Application.DTOs.Checkout
{
    public class CheckoutSummaryDto
    {
        public List<CheckoutItemDto> Items { get; set; }
            = new();

        public ShippingAddressResponseDto?
            ShippingAddress
        { get; set; }

        public decimal SubTotal { get; set; }

        public decimal ShippingCharge { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public int TotalItems { get; set; }
    }
}