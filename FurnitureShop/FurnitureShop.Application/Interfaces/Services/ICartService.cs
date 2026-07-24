using FurnitureShop.Application.DTOs.Cart;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<AddToCartResponseDto> AddToCartAsync(
            Guid userId,
            AddToCartRequestDto request);
        Task<CartResponseDto?> GetMyCartAsync(Guid userId);
        Task<CartResponseDto?> GetUserCartAsync(Guid userId);
        Task<bool> UpdateItemAsync(
            Guid userId,
            UpdateCartItemRequestDto request);
        Task RemoveItemAsync(
            Guid userId,
            Guid productId);
        Task ClearCartAsync(Guid userId);
        Task<int> GetCartQuantityAsync(Guid userId);
        Task<int> GetCartItemCountAsync(Guid userId);
    }
}