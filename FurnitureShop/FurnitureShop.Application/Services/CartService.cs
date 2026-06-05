using FurnitureShop.Application.DTOs.Cart;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;

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

        public async Task<AddToCartResponseDto> AddToCartAsync(
            Guid userId,
            AddToCartRequestDto request)
        {
            if (request == null ||
                request.ProductId == Guid.Empty ||
                request.Quantity <= 0)
            {
                throw new ArgumentException(
                    "Invalid cart request");
            }

            var product = await _productRepository
                .GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException(
                    "Product does not exist");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    "Product is unavailable");
            }

            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _cartRepository.AddAsync(cart);
            }

            var item = await _cartRepository
                .GetCartItemAsync(
                    cart.Id,
                    request.ProductId);

            var requestedQuantity =
                item == null
                    ? request.Quantity
                    : item.Quantity + request.Quantity;

            if (requestedQuantity > product.StockQuantity)
            {
                throw new InvalidOperationException(
                    $"Only {product.StockQuantity} item(s) available.");
            }

            if (item == null)
            {
                item = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };

                await _cartRepository.AddCartItemAsync(item);
            }
            else
            {
                item.Quantity += request.Quantity;
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

        public async Task<CartResponseDto?> GetMyCartAsync(
            Guid userId)
        {
            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            return Map(cart);
        }

        public async Task<CartResponseDto?> GetCartByIdAsync(
            Guid cartId)
        {
            var cart = await _cartRepository
                .GetByIdAsync(cartId);

            return Map(cart);
        }

        public async Task RemoveItemAsync(
            Guid userId,
            Guid productId)
        {
            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            if (cart == null)
                return;

            var item = cart.Items
                .FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                return;

            cart.Items.Remove(item);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }

        public async Task<bool> UpdateItemAsync(
            Guid userId,
            UpdateCartItemRequestDto request)
        {
            if (request.Quantity < 0)
            {
                throw new ArgumentException(
                    "Quantity cannot be negative");
            }

            var cart = await _cartRepository
                .GetByUserIdAsync(userId);

            if (cart == null)
                return false;

            var item = cart.Items
                .FirstOrDefault(i =>
                    i.ProductId == request.ProductId);

            if (item == null)
                return false;

            if (request.Quantity == 0)
            {
                cart.Items.Remove(item);

                await _cartRepository.SaveChangesAsync();

                return true;
            }

            var product = await _productRepository
                .GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException(
                    "Product not found");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    "Product is unavailable");
            }

            if (request.Quantity > product.StockQuantity)
            {
                throw new InvalidOperationException(
                    $"Only {product.StockQuantity} item(s) available.");
            }

            item.Quantity = request.Quantity;

            await _cartRepository.SaveChangesAsync();

            return true;
        }

        private static CartResponseDto? Map(Cart? cart)
        {
            if (cart == null)
                return null;

            var activeItems = cart.Items
                .Where(i =>
                    i.Product != null &&
                    i.Product.IsActive)
                .ToList();

            return new CartResponseDto
            {
                CartId = cart.Id,

                Items = activeItems
                    .Select(i => new CartItemResponseDto
                    {
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = i.Product.Price,
                        Imageurl = i.Product.ImageUrl,
                        Quantity = i.Quantity
                    })
                    .ToList(),

                TotalAmount = activeItems.Sum(i =>
                    i.Product.Price * i.Quantity)
            };
        }
    }
}