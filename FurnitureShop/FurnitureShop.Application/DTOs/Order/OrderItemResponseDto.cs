using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Order
{
    public class OrderItemResponseDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
