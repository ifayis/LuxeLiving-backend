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

        // CREATE
        public async Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request)
        {
            if (await _productRepository.ExistsByNameAsync(request.Name))
                throw new InvalidOperationException(ErrorMessages.AlreadyExists);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                IsActive = true
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return MapToDto(product);
        }

        // GET BY ID
        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        // GET BY CATEGORY
        public async Task<List<ProductResponseDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToDto).ToList();
        }

        // GET ALL
        public async Task<List<ProductResponseDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        // ACTIVATE
        public async Task ActivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException(ErrorMessages.NotFound);

            product.IsActive = true;
            await _productRepository.SaveChangesAsync();
        }

        // DEACTIVATE
        public async Task DeactivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new KeyNotFoundException(ErrorMessages.NotFound);

            product.IsActive = false;
            await _productRepository.SaveChangesAsync();
        }

        // MAPPER (PRIVATE, CLEAN)
        private static ProductResponseDto MapToDto(Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive
            };
    }
}
