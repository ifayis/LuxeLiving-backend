using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces;
using FurnitureShop.Domain.Enitities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<ApiResponse<string>> AddToCartAsync(Guid userId, AddToCartRequestDto request)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<CartItem>()
                };
            }

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }

            if (cart.Id == Guid.Empty)
                await _cartRepository.AddAsync(cart);
            else
                await _cartRepository.UpdateAsync(cart);

            return ApiResponse<string>.Success("Item added to cart");
        }

        public async Task<ApiResponse<CartResponseDto>> GetMyCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                return ApiResponse<CartResponseDto>.Fail("Cart is empty", 404);
            }

            var response = new CartResponseDto
            {
                CartId = cart.Id,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            return ApiResponse<CartResponseDto>.Success(response);
        }

        public async Task<ApiResponse<string>> UpdateItemAsync(Guid userId, UpdateCartItemDto request)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return ApiResponse<string>.Fail("Cart not found", 404);

            var item = cart.Items.FirstOrDefault(i => i.Id == request.CartItemId);

            if (item == null)
                return ApiResponse<string>.Fail("Item not found", 404);

            item.Quantity = request.Quantity;

            await _cartRepository.UpdateAsync(cart);

            return ApiResponse<string>.Success("Cart item updated");
        }

        public async Task<ApiResponse<string>> RemoveItemAsync(Guid userId, Guid itemId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return ApiResponse<string>.Fail("Cart not found", 404);

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                return ApiResponse<string>.Fail("Item not found", 404);

            cart.Items.Remove(item);

            await _cartRepository.UpdateAsync(cart);

            return ApiResponse<string>.Success("Item removed from cart");
        }

        public async Task<ApiResponse<string>> ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
                return ApiResponse<string>.Fail("Cart already empty", 400);

            await _cartRepository.ClearCartAsync(cart.Id);

            return ApiResponse<string>.Success("Cart cleared");
        }
    }
}
