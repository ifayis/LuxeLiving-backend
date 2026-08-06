using FurnitureShop.Application.DTOs.Product;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request);
        Task<ProductResponseDto?> GetByIdAsync(Guid productId);
        Task<ProductResponseDto?> GetBySlugAsync(string slug);
        Task<ProductResponseDto?> GetBySkuAsync(string sku);
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<List<ProductResponseDto>> GetActiveAsync();
        Task<List<ProductResponseDto>> GetByCategoryAsync(Guid categoryId);
        Task<List<ProductResponseDto>> GetFeaturedProductsAsync();
        Task<List<ProductResponseDto>> GetNewArrivalProductsAsync();
        Task<List<ProductResponseDto>> GetBestSellerProductsAsync();
        Task<List<ProductResponseDto>> SearchAsync(string keyword);
        Task<ProductResponseDto> UpdateAsync(
            Guid productId,
            UpdateProductRequestDto request);
        Task ActivateAsync(Guid productId);
        Task DeactivateAsync(Guid productId);
        Task DeleteAsync(Guid productId);
    }
}