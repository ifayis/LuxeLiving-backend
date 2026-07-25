using FurnitureShop.Application.DTOs.Checkout;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        #region Checkout Summary

        Task<CheckoutSummaryDto> GetSummaryAsync(
            Guid userId);

        #endregion

        #region Checkout

        Task<PaymentResponseDto> CheckoutAsync(
            Guid userId,
            CheckoutRequestDto request);

        #endregion
    }
}