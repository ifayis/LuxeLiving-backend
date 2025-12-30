using System;
using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Cart
{
    public class AddToCartRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
