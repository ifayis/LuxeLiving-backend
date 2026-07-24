using FurnitureShop.Application.DTOs.Product;

namespace FurnitureShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        #region Create

        Task<ProductResponseDto> CreateAsync(
            CreateProductRequestDto request);

        #endregion

        #region Read

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

        #endregion

        #region Update

        Task<ProductResponseDto> UpdateAsync(
            Guid productId,
            UpdateProductRequestDto request);

        Task ActivateAsync(Guid productId);

        Task DeactivateAsync(Guid productId);

        #endregion

        #region Delete

        Task DeleteAsync(Guid productId);

        #endregion
    }
}