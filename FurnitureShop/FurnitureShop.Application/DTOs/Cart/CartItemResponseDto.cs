using System;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Imageurl { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
