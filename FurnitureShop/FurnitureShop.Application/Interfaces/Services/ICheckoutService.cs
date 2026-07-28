using FurnitureShop.Application.DTOs.Checkout;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutSummaryDto> GetSummaryAsync(Guid userId);
        Task<PaymentResponseDto> CheckoutAsync(Guid userId, CheckoutRequestDto request);
    }
}