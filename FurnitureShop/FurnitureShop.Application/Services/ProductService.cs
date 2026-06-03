using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request)
        {
            if (await _productRepository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException(ErrorMessages.AlreadyExists);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                IsActive = true,
                StockQuantity = request.StockQuantity
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<List<ProductResponseDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToDto).ToList();
        }

        public async Task<List<ProductResponseDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task ActivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException(ErrorMessages.NotFound);

            product.IsActive = true;
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeactivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException(ErrorMessages.NotFound);

            product.IsActive = false;
            await _productRepository.SaveChangesAsync();
        }

        public async Task<ApiResponse<object>> DeleteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return ApiResponse<object>.Fail(
                    ErrorMessages.InvalidId,
                    400);
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponse<object>.Fail(
                    ErrorMessages.NotFound,
                    404);
            }

            await _productRepository.DeleteAsync(product);
            await _productRepository.SaveChangesAsync();

            return ApiResponse<object>.Success(
                null,
                ResponseMessages.ProductDeleted,
                200);
        }

        public async Task<ApiResponse<object>> DeleteAllAsync()
        {
            await _productRepository.DeleteAllAsync();
            await _productRepository.SaveChangesAsync();

            return ApiResponse<object>.Success(
                null,
                ResponseMessages.ProductsDeleted,
                200);
        }

        public async Task<bool> UpdateAsync(Guid productId, UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return false;

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;
            product.StockQuantity = request.StockQuantity;


            await _productRepository.SaveChangesAsync();
            return true;
        }
        private static ProductResponseDto MapToDto(Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                StockQuantity = product.StockQuantity
            };
    }
}
