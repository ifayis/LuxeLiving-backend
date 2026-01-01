using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };
        }
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _productRepository.GetByCategoryIdAsync(categoryId);
        }
        public async Task CreateAsync(CreateProductRequestDto request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId
            };

            await _productRepository.AddAsync(product);
        }

        public async Task<ApiResponse<IEnumerable<ProductResponseDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAll();

            if (products == null || !products.Any())
            {
                return ApiResponse<IEnumerable<ProductResponseDto>>
                    .Fail("No products found", 404);
            }

            var productDtos = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl
            });

            return ApiResponse<IEnumerable<ProductResponseDto>>
                .Success(productDtos, "Products retrieved successfully");
        }

        public async Task ActivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            product.IsActive = true;
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeactivateProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            product.IsActive = false;
            await _productRepository.SaveChangesAsync();
        }


    }
}
