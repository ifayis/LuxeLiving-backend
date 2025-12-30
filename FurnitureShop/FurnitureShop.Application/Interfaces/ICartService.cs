using System;
using System.Threading.Tasks;
using FurnitureShop.Application.DTOs.Cart;

namespace FurnitureShop.Application.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(Guid userId, AddToCartRequestDto request);
        Task<CartResponseDto?> GetMyCartAsync(Guid userId);
        Task<CartResponseDto?> GetCartByIdAsync(Guid cartId);
        Task RemoveItemAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
    }
}
