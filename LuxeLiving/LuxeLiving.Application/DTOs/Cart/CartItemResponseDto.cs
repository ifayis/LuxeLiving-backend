namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;

        public bool IsAvailable { get; set; }

        public int AvailableStock { get; set; }
    }
}