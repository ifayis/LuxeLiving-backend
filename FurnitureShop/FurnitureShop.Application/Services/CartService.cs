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
        private readonly IProductRepository _productRepository;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<AddToCartResponseDto> AddToCartAsync(Guid userId, AddToCartRequestDto request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new InvalidOperationException("Product does not exist");

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                _cartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items
                .FirstOrDefault(i => i.ProductId == request.ProductId);
           
            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

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

            return new AddToCartResponseDto
            {
                CartId = cart.Id,
                Product = new AddedCartProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = item.Quantity
                }
            };
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

        public async Task<bool> UpdateItemAsync(Guid userId, UpdateCartItemRequestDto request)
        {
            if (request.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                return false;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return false;

            if (request.Quantity == 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = request.Quantity;
            }
            await _cartRepository.SaveChangesAsync();
            return true;
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
