using FurnitureShop.Application.Common;
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
            if (request.ProductId == Guid.Empty)
                throw new ArgumentException(ErrorMessages.InvalidProduct);

            if (request.Quantity <= 0)
                throw new ArgumentException(ErrorMessages.InvalidQuantity);

            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
                throw new KeyNotFoundException(ErrorMessages.ProductNotFound);

            if (!product.IsActive)
                throw new InvalidOperationException(ErrorMessages.ProductUnavailable);

            if (product.StockQuantity <= 0)
                throw new InvalidOperationException(ErrorMessages.OutOfStock);

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastActivityAt = DateTime.UtcNow
                };

                await _cartRepository.AddAsync(cart);
            }

            var cartItem = await _cartRepository.GetCartItemAsync(
                cart.Id,
                request.ProductId);

            if (cartItem == null)
            {
                if (request.Quantity > product.StockQuantity)
                    throw new InvalidOperationException(
                        $"Only {product.StockQuantity} item(s) available.");

                cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = request.Quantity
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }
            else
            {
                var newQuantity = cartItem.Quantity + request.Quantity;

                if (newQuantity > product.StockQuantity)
                    throw new InvalidOperationException(
                        $"Only {product.StockQuantity} item(s) available.");

                cartItem.Quantity = newQuantity;
            }

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(cart);

            await _cartRepository.SaveChangesAsync();

            var itemCount = await _cartRepository.GetCartItemCountAsync(cart.Id);

            var refreshedCart = await _cartRepository.GetByUserIdAsync(userId);

            var cartTotal = refreshedCart?.Items
                .Where(x => x.Product != null && x.Product.IsActive)
                .Sum(x => x.Product.Price * x.Quantity) ?? 0;

            return new AddToCartResponseDto
            {
                CartId = cart.Id,

                CartItemCount = itemCount,

                CartTotal = cartTotal,

                Product = new AddedCartProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    UnitPrice = product.Price,
                    Quantity = cartItem.Quantity
                }
            };
        }

        public async Task<CartResponseDto?> GetMyCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return null;

            return await BuildCartResponseAsync(cart);
        }

        public async Task<CartResponseDto?> GetUserCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return null;

            return await BuildCartResponseAsync(cart);
        }

        public async Task<bool> UpdateItemAsync(
            Guid userId,
            UpdateCartItemRequestDto request)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return false;

            var item = cart.Items.FirstOrDefault(x =>
                x.ProductId == request.ProductId);

            if (item == null)
                return false;

            var product = await _productRepository.GetByIdAsync(
                request.ProductId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            if (!product.IsActive)
                throw new InvalidOperationException(
                    ErrorMessages.ProductUnavailable);

            if (product.StockQuantity <= 0)
                throw new InvalidOperationException(
                    ErrorMessages.OutOfStock);

            if (request.Quantity > product.StockQuantity)
                throw new InvalidOperationException(
                    $"Only {product.StockQuantity} item(s) available.");

            item.Quantity = request.Quantity;

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(cart);

            await _cartRepository.SaveChangesAsync();

            return true;
        }

        public async Task RemoveItemAsync(
            Guid userId,
            Guid productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                throw new KeyNotFoundException(ErrorMessages.CartNotFound);

            var item = cart.Items.FirstOrDefault(x =>
                x.ProductId == productId);

            if (item == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CartItemNotFound);

            await _cartRepository.RemoveCartItemAsync(item);

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(cart);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return;

            await _cartRepository.ClearCartAsync(userId);

            cart.UpdatedAt = DateTime.UtcNow;
            cart.LastActivityAt = DateTime.UtcNow;

            await _cartRepository.UpdateAsync(cart);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task<int> GetCartQuantityAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return 0;

            return cart.Items.Sum(x => x.Quantity);
        }

        public async Task<int> GetCartItemCountAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return 0;

            return cart.Items.Count;
        }


        private async Task<CartResponseDto> BuildCartResponseAsync(Cart cart)
        {
            var invalidItems = cart.Items
                .Where(x =>
                    x.Product == null ||
                    !x.Product.IsActive ||
                    x.Product.StockQuantity <= 0)
                .ToList();

            foreach (var item in invalidItems)
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }

            if (invalidItems.Count > 0)
            {
                cart.UpdatedAt = DateTime.UtcNow;
                cart.LastActivityAt = DateTime.UtcNow;

                await _cartRepository.UpdateAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var items = cart.Items
                .Except(invalidItems)
                .ToList();

            var subTotal = items.Sum(x =>
                x.Product!.Price * x.Quantity);

            return new CartResponseDto
            {
                CartId = cart.Id,

                Items = items.Select(x => new CartItemResponseDto
                {
                    ProductId = x.ProductId,
                    Name = x.Product!.Name,
                    ImageUrl = x.Product.ImageUrl,
                    UnitPrice = x.Product.Price,
                    Quantity = x.Quantity,
                    AvailableStock = x.Product.StockQuantity,
                    IsAvailable = x.Product.IsActive
                }).ToList(),

                TotalUniqueItems = items.Count,

                TotalQuantity = items.Sum(x => x.Quantity),

                SubTotal = subTotal,

                Discount = 0,

                ShippingCharge = 0,

                Tax = 0,

                GrandTotal = subTotal
            };
        }
    }
}