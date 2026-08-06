namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartResponseDto
    {
        public Guid CartId { get; set; }

        public List<CartItemResponseDto> Items { get; set; } = new();

        public int TotalUniqueItems { get; set; }

        public int TotalQuantity { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal ShippingCharge { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public bool IsEmpty => !Items.Any();
    }
}