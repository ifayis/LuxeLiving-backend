using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Checkout
{
    public class CheckoutResponseDto
    {
        public List<CheckOutItemDto> Items { get; set; } = new();
        public decimal GrossTotal { get; set; }
    }
}
