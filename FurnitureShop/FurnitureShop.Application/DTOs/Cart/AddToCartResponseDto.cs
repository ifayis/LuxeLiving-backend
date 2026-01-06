using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class AddToCartResponseDto
    {
        public Guid CartId { get; set; }
        public AddedCartProductDto Product { get; set; } = null!;
    }
}
