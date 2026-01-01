using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
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

        public async Task AddToCartAsync(Guid userId, AddToCartRequestDto request)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });

                await _cartRepository.AddAsync(cart);
                return;
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

            await _cartRepository.SaveChangesAsync();
        }

        public async Task<CartResponseDto?> GetMyCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            return Map(cart);
        }

        public async Task<CartResponseDto?> GetCartByIdAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            return Map(cart);
        }

        public async Task RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            cart.Items.Remove(item);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            cart.Items.Clear();
            await _cartRepository.SaveChangesAsync();
        }

        private static CartResponseDto? Map(Cart? cart)
        {
            if (cart == null) return null;

            return new CartResponseDto
            {
                CartId = cart.Id,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
