using System;
using System.Threading.Tasks;
using FurnitureShop.Application.DTOs.Cart;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<AddToCartResponseDto> AddToCartAsync(Guid userId, AddToCartRequestDto request);
        Task<CartResponseDto?> GetMyCartAsync(Guid userId);
        Task<CartResponseDto?> GetCartByIdAsync(Guid cartId);
        Task RemoveItemAsync(Guid userId, Guid productId);
        Task<bool> UpdateItemAsync(Guid userId, UpdateCartItemRequestDto request);
        Task ClearCartAsync(Guid userId);
    }
}
