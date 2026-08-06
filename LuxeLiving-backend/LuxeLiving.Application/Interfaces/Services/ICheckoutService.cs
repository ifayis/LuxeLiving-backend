using LuxeLiving.Application.DTOs.Checkout;

namespace LuxeLiving.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutSummaryDto> GetSummaryAsync(Guid userId);
        Task<PaymentResponseDto> CheckoutAsync(Guid userId, CheckoutRequestDto request);
    }
}