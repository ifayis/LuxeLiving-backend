using FurnitureShop.Application.Common;
using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request);
        Task<ProductResponseDto?> GetProductByIdAsync(Guid id);
        Task<List<ProductResponseDto>> GetProductsByCategoryAsync(Guid categoryId);
        Task<List<ProductResponseDto>> GetAllProducts();
        Task ActivateProductAsync(Guid productId);
        Task DeactivateProductAsync(Guid productId);
    }
}
