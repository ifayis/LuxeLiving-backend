using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using System.Text.RegularExpressions;

namespace FurnitureShop.Application.Services
{
    public partial class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductResponseDto> CreateAsync(
            CreateProductRequestDto request)
        {
            var category = await _categoryRepository
                .GetByIdAsync(request.CategoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CategoryNotFound);

            if (!category.IsActive)
                throw new InvalidOperationException(
                    ErrorMessages.CategoryInactive);

            var productName = request.Name.Trim();

            if (await _productRepository.ExistsByNameAsync(productName))
                throw new InvalidOperationException(
                    ErrorMessages.ProductAlreadyExists);

            if (request.Price > request.OriginalPrice)
                throw new InvalidOperationException(
                    ErrorMessages.InvalidProductPrice);

            var slug = await GenerateUniqueSlugAsync(productName);

            var sku = await GenerateSkuAsync();

            var discountPercentage =
                request.OriginalPrice == 0
                    ? 0
                    : Math.Round(
                        ((request.OriginalPrice - request.Price)
                        / request.OriginalPrice) * 100,
                        2);

            var product = new Product
            {
                Id = Guid.NewGuid(),

                Name = productName,

                Slug = slug,

                SKU = sku,

                Description = request.Description.Trim(),

                OriginalPrice = request.OriginalPrice,

                Price = request.Price,

                DiscountPercentage = discountPercentage,

                ImageUrl = request.ImageUrl?.Trim(),

                CategoryId = request.CategoryId,

                StockQuantity = request.StockQuantity,

                IsFeatured = request.IsFeatured,

                IsNewArrival = request.IsNewArrival,

                IsBestSeller = request.IsBestSeller,

                IsActive = true,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);

            await _productRepository.SaveChangesAsync();

            product.Category = category;

            return Map(product);
        }

        private async Task<string> GenerateUniqueSlugAsync(
            string name)
        {
            var slug = GenerateSlug(name);

            var originalSlug = slug;

            var counter = 1;

            while (await _productRepository.ExistsBySlugAsync(slug))
            {
                slug = $"{originalSlug}-{counter++}";
            }

            return slug;
        }

        private static string GenerateSlug(string text)
        {
            text = text.Trim().ToLowerInvariant();

            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            text = Regex.Replace(text, @"\s+", "-");

            text = Regex.Replace(text, @"-+", "-");

            return text;
        }

        private async Task<string> GenerateSkuAsync()
        {
            while (true)
            {
                var sku = $"LUX-{Random.Shared.Next(100000, 999999)}";

                if (!await _productRepository.ExistsBySkuAsync(sku))
                    return sku;
            }
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            return product == null
                ? null
                : Map(product);
        }

        public async Task<ProductResponseDto?> GetBySlugAsync(string slug)
        {
            var product = await _productRepository.GetBySlugAsync(slug);

            return product == null
                ? null
                : Map(product);
        }

        public async Task<ProductResponseDto?> GetBySkuAsync(string sku)
        {
            var product = await _productRepository.GetBySkuAsync(sku);

            return product == null
                ? null
                : Map(product);
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> GetActiveAsync()
        {
            var products = await _productRepository.GetActiveAsync();

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> GetByCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CategoryNotFound);

            var products = await _productRepository
                .GetByCategoryAsync(categoryId);

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> GetFeaturedProductsAsync()
        {
            var products = await _productRepository
                .GetFeaturedProductsAsync();

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> GetNewArrivalProductsAsync()
        {
            var products = await _productRepository
                .GetNewArrivalProductsAsync();

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> GetBestSellerProductsAsync()
        {
            var products = await _productRepository
                .GetBestSellerProductsAsync();

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<List<ProductResponseDto>> SearchAsync(string keyword)
        {
            keyword = keyword?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(keyword))
                return new List<ProductResponseDto>();

            var products = await _productRepository
                .SearchAsync(keyword);

            return products
                .Select(Map)
                .ToList();
        }

        public async Task<ProductResponseDto> UpdateAsync(
    Guid productId,
    UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
                throw new KeyNotFoundException(
                    ErrorMessages.CategoryNotFound);

            if (!category.IsActive)
                throw new InvalidOperationException(
                    ErrorMessages.CategoryInactive);

            var productName = request.Name.Trim();

            if (await _productRepository.ExistsByNameAsync(
                productName,
                productId))
            {
                throw new InvalidOperationException(
                    ErrorMessages.ProductAlreadyExists);
            }

            if (!string.Equals(
                    product.Name,
                    productName,
                    StringComparison.OrdinalIgnoreCase))
            {
                product.Slug = await GenerateUniqueSlugAsync(productName);
            }

            if (request.Price > request.OriginalPrice)
                throw new InvalidOperationException(
                    ErrorMessages.InvalidProductPrice);

            product.Name = productName;
            product.Description = request.Description.Trim();
            product.OriginalPrice = request.OriginalPrice;
            product.Price = request.Price;

            product.DiscountPercentage =
                request.OriginalPrice == 0
                    ? 0
                    : Math.Round(
                        ((request.OriginalPrice - request.Price)
                        / request.OriginalPrice) * 100,
                        2);

            product.ImageUrl = request.ImageUrl?.Trim();

            product.CategoryId = request.CategoryId;

            product.StockQuantity = request.StockQuantity;

            product.IsFeatured = request.IsFeatured;

            product.IsNewArrival = request.IsNewArrival;

            product.IsBestSeller = request.IsBestSeller;

            product.IsActive = request.IsActive;

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);

            await _productRepository.SaveChangesAsync();

            product.Category = category;

            return Map(product);
        }

        public async Task ActivateAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            product.IsActive = true;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);

            await _productRepository.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);

            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException(
                    ErrorMessages.ProductNotFound);

            await _productRepository.DeleteAsync(product);

            await _productRepository.SaveChangesAsync();
        }

        private static ProductResponseDto Map(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,

                Name = product.Name,

                Slug = product.Slug,

                SKU = product.SKU,

                Description = product.Description,

                OriginalPrice = product.OriginalPrice,

                Price = product.Price,

                DiscountPercentage = product.DiscountPercentage,

                ImageUrl = product.ImageUrl,

                CategoryId = product.CategoryId,

                CategoryName = product.Category?.Name ?? string.Empty,

                StockQuantity = product.StockQuantity,

                IsActive = product.IsActive,

                IsFeatured = product.IsFeatured,

                IsNewArrival = product.IsNewArrival,

                IsBestSeller = product.IsBestSeller,

                CreatedAt = product.CreatedAt
            };
        }
    }
}