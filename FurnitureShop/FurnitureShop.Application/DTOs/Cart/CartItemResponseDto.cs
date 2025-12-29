using System;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}
