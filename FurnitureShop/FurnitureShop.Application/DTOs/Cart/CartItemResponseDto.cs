using System;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
