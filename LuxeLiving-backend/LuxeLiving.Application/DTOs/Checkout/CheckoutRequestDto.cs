using LuxeLiving.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LuxeLiving.Application.DTOs.Checkout
{
    public class CheckoutRequestDto
    {
        [Required]
        public Guid ShippingAddressId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
}