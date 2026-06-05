using System;
using System.Collections.Generic;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class CartResponseDto
    {
        public Guid CartId { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
