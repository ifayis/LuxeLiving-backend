using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _WishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public WishlistService(
            IWishlistRepository WishlistRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository)
        {
            _WishlistRepository = WishlistRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<WishlistResponseDto> AddAsync(
            Guid userId,
            AddToWishlistRequestDto request)
        {
            if (request == null || request.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid wishlist request");

            var product = await _productRepository
                .GetByIdAsync(request.ProductId);

            if (product == null)
                throw new ArgumentException("Product does not exist");

            if (!product.IsActive)
                throw new InvalidOperationException(
                    "Product is unavailable");

            var wishlist = await _WishlistRepository
                .GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _WishlistRepository.AddAsync(wishlist);
            }

            if (wishlist.Items.Any(i =>
                i.ProductId == request.ProductId))
            {
                throw new InvalidOperationException(
                    "Product already in wishlist");
            }

            var wishlistItem = new WishlistItem
            {
                Id = Guid.NewGuid(),
                WishlistId = wishlist.Id,
                ProductId = request.ProductId
            };

            _WishlistRepository.AddItem(wishlistItem);

            await _WishlistRepository.SaveChangesAsync();

            var updatedWishlist =
                await _WishlistRepository.GetByUserIdAsync(userId);

            return Map(updatedWishlist!);
        }

        public async Task<WishlistResponseDto?> GetMyAsync(Guid userId)
        {
            var wishlist = await _WishlistRepository
                .GetByUserIdAsync(userId);

            return wishlist == null
                ? null
                : Map(wishlist);
        }

        public async Task<WishlistResponseDto?> GetByUserIdAsync(Guid userId)
        {
            var wishlist = await _WishlistRepository
                .GetByUserIdAsync(userId);

            return wishlist == null
                ? null
                : Map(wishlist);
        }

        public async Task RemoveItemAsync(
            Guid userId,
            Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException(
                    "Invalid item id");

            var wishlist =
                await _WishlistRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException(
                    "Wishlist not found");

            var item = wishlist.Items
                .FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException(
                    "Wishlist item not found");

            _WishlistRepository.RemoveItem(item);

            await _WishlistRepository.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid userId)
        {
            var wishlist =
                await _WishlistRepository.GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException(
                    "Wishlist not found");

            _WishlistRepository.RemoveAll(wishlist);

            await _WishlistRepository.SaveChangesAsync();
        }

        public async Task MoveToCartAsync(Guid userId)
        {
            var wishlist =
                await _WishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null || !wishlist.Items.Any())
            {
                throw new InvalidOperationException(
                    "Wishlist is empty");
            }

            var cart =
                await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                await _cartRepository.AddAsync(cart);
            }

            var movedItems = new List<WishlistItem>();

            foreach (var item in wishlist.Items)
            {
                var product =
                    await _productRepository.GetByIdAsync(
                        item.ProductId);

                if (product == null)
                    continue;

                if (!product.IsActive)
                    continue;

                var existingCartItem = cart.Items
                    .FirstOrDefault(c =>
                        c.ProductId == item.ProductId);

                var newQuantity =
                    existingCartItem == null
                        ? 1
                        : existingCartItem.Quantity + 1;

                if (newQuantity > product.StockQuantity)
                    continue;

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += 1;
                }
                else
                {
                    await _cartRepository.AddCartItemAsync(
                        new CartItem
                        {
                            Id = Guid.NewGuid(),
                            CartId = cart.Id,
                            ProductId = item.ProductId,
                            Quantity = 1
                        });
                }

                movedItems.Add(item);
            }

            foreach (var item in movedItems)
            {
                _WishlistRepository.RemoveItem(item);
            }

            await _WishlistRepository.SaveChangesAsync();
            await _cartRepository.SaveChangesAsync();
        }

        private static WishlistResponseDto Map(Wishlist wishlist)
        {
            var activeItems = wishlist.Items
                .Where(i =>
                    i.Product != null &&
                    i.Product.IsActive)
                .ToList();

            return new WishlistResponseDto
            {
                Id = wishlist.Id,

                ItemCount = activeItems.Count,

                Items = activeItems
                    .Select(i => new WishlistItemResponseDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Price = i.Product.Price,
                        ImageUrl = i.Product.ImageUrl
                    })
                    .ToList()
            };
        }
    }
}