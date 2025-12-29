using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Cart;
using System;
using System.Threading.Tasks;

public interface ICartService
{
    Task<ApiResponse<string>> AddToCartAsync(Guid userId, AddToCartRequestDto request);

    Task<ApiResponse<CartResponseDto>> GetMyCartAsync(Guid userId);

    Task<ApiResponse<string>> UpdateItemAsync(Guid userId, UpdateCartItemDto request);

    Task<ApiResponse<string>> RemoveItemAsync(Guid userId, Guid itemId);

    Task<ApiResponse<string>> ClearCartAsync(Guid userId);
}
