using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Wishlist;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Services
{
    public partial class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public WishlistService(
            IWishlistRepository wishlistRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<WishlistResponseDto> AddAsync(
            Guid userId,
            AddToWishlistRequestDto request)
        {
            var product = await _productRepository
                .GetByIdAsync(request.ProductId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            if (!product.IsActive)
                throw new InvalidOperationException(
                    ErrorMessages.ProductInactive);

            if (product.Category == null || !product.Category.IsActive)
                throw new InvalidOperationException(
                    ErrorMessages.CategoryInactive);

            var wishlist = await _wishlistRepository
                .GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _wishlistRepository.AddAsync(wishlist);

                await _wishlistRepository.SaveChangesAsync();
            }

            if (await _wishlistRepository.ExistsAsync(
                    wishlist.Id,
                    request.ProductId))
            {
                throw new InvalidOperationException(
                    ErrorMessages.ProductAlreadyInWishlist);
            }

            await _wishlistRepository.AddWishlistItemAsync(
                new WishlistItem
                {
                    Id = Guid.NewGuid(),
                    WishlistId = wishlist.Id,
                    ProductId = request.ProductId,
                    CreatedAt = DateTime.UtcNow
                });

            wishlist.UpdatedAt = DateTime.UtcNow;

            await _wishlistRepository.UpdateAsync(wishlist);

            await _wishlistRepository.SaveChangesAsync();

            var updatedWishlist =
                await _wishlistRepository.GetByUserIdAsync(userId);

            return Map(updatedWishlist!);
        }

        public async Task<WishlistResponseDto?> GetMyWishlistAsync(
            Guid userId)
        {
            var wishlist =
                await _wishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null)
                return null;

            return Map(wishlist);
        }

        public async Task<WishlistResponseDto?> GetWishlistByUserIdAsync(
            Guid userId)
        {
            var wishlist =
                await _wishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null)
                return null;

            return Map(wishlist);
        }

        public async Task<WishlistResponseDto?> GetWishlistByIdAsync(
            Guid wishlistId)
        {
            var wishlist =
                await _wishlistRepository.GetByIdAsync(wishlistId);

            if (wishlist == null)
                return null;

            return Map(wishlist);
        }

        public async Task RemoveItemAsync(
    Guid userId,
    Guid wishlistItemId)
        {
            var wishlist = await _wishlistRepository
                .GetByUserIdAsync(userId);

            if (wishlist == null)
                throw new KeyNotFoundException(
                    ErrorMessages.WishlistNotFound);

            var wishlistItem = await _wishlistRepository
                .GetWishlistItemByIdAsync(wishlistItemId);

            if (wishlistItem == null ||
                wishlistItem.WishlistId != wishlist.Id)
            {
                throw new KeyNotFoundException(
                    ErrorMessages.WishlistItemNotFound);
            }

            await _wishlistRepository
                .RemoveWishlistItemAsync(wishlistItem);

            wishlist.UpdatedAt = DateTime.UtcNow;

            await _wishlistRepository.UpdateAsync(wishlist);

            await _wishlistRepository.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid userId)
        {
            var wishlist = await _wishlistRepository
                .GetByUserIdAsync(userId);

            if (wishlist == null)
                throw new KeyNotFoundException(
                    ErrorMessages.WishlistNotFound);

            await _wishlistRepository
                .ClearWishlistAsync(wishlist);

            await _wishlistRepository.UpdateAsync(wishlist);

            await _wishlistRepository.SaveChangesAsync();
        }

        private static WishlistResponseDto Map(
            Wishlist wishlist)
        {
            var items = wishlist.Items
                .Where(x =>
                    x.Product != null &&
                    x.Product.IsActive &&
                    x.Product.Category != null &&
                    x.Product.Category.IsActive)
                .ToList();

            return new WishlistResponseDto
            {
                WishlistId = wishlist.Id,

                TotalItems = items.Count,

                Items = items
                    .Select(x => new WishlistItemResponseDto
                    {
                        WishlistItemId = x.Id,

                        ProductId = x.ProductId,

                        ProductName = x.Product.Name,

                        Slug = x.Product.Slug,

                        SKU = x.Product.SKU,

                        CategoryName = x.Product.Category.Name,

                        OriginalPrice = x.Product.OriginalPrice,

                        Price = x.Product.Price,

                        DiscountPercentage =
                            x.Product.DiscountPercentage,

                        ImageUrl = x.Product.ImageUrl,

                        StockQuantity =
                            x.Product.StockQuantity,

                        IsAvailable =
                            x.Product.StockQuantity > 0 &&
                            x.Product.IsActive,

                        AddedAt = x.CreatedAt
                    })
                    .OrderByDescending(x => x.AddedAt)
                    .ToList()
            };
        }

        public async Task MoveToCartAsync(Guid userId)
        {
            var wishlist = await _wishlistRepository
                .GetByUserIdAsync(userId);

            if (wishlist == null || !wishlist.Items.Any())
                throw new InvalidOperationException(
                    ErrorMessages.WishlistEmpty);

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

                await _cartRepository.SaveChangesAsync();
            }

            var movedItems = new List<WishlistItem>();

            foreach (var wishlistItem in wishlist.Items)
            {
                var product = wishlistItem.Product;

                if (product == null)
                    continue;

                if (!product.IsActive)
                    continue;

                if (product.Category == null ||
                    !product.Category.IsActive)
                    continue;

                if (product.StockQuantity <= 0)
                    continue;

                var cartItem = await _cartRepository
                    .GetCartItemAsync(
                        cart.Id,
                        product.Id);

                if (cartItem == null)
                {
                    await _cartRepository.AddCartItemAsync(
                        new CartItem
                        {
                            Id = Guid.NewGuid(),
                            CartId = cart.Id,
                            ProductId = product.Id,
                            Quantity = 1
                        });
                }
                else
                {
                    if (cartItem.Quantity >= product.StockQuantity)
                        continue;

                    cartItem.Quantity++;
                }

                movedItems.Add(wishlistItem);
            }

            foreach (var item in movedItems)
            {
                await _wishlistRepository
                    .RemoveWishlistItemAsync(item);
            }

            wishlist.UpdatedAt = DateTime.UtcNow;

            await _wishlistRepository.UpdateAsync(wishlist);

            await _cartRepository.SaveChangesAsync();

            await _wishlistRepository.SaveChangesAsync();
        }
    }
}