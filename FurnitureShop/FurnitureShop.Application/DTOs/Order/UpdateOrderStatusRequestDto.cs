using FurnitureShop.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Order
{
    public class UpdateOrderStatusRequestDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
