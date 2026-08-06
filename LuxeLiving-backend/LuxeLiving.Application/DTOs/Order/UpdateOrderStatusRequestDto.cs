using LuxeLiving.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LuxeLiving.Application.DTOs.Order
{
    public class UpdateOrderStatusRequestDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
