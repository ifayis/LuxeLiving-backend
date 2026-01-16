using FurnitureShop.Application.DTOs.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public class ICheckoutService
    {
        Task<CheckoutResponseDto> GetCheckoutAsync(Guid userId);
        Task ExecutePaymentAsync(Guid userId, PaymentRequestDto request)
    }
}
