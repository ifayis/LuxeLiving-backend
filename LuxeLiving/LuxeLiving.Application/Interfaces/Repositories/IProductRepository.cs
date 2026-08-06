using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> GetBySlugAsync(string slug);
        Task<Product?> GetBySkuAsync(string sku);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetActiveAsync();
        Task<List<Product>> GetByCategoryAsync(Guid categoryId);
        Task<List<Product>> GetFeaturedProductsAsync();
        Task<List<Product>> GetNewArrivalProductsAsync();
        Task<List<Product>> GetBestSellerProductsAsync();
        Task<List<Product>> SearchAsync(string keyword);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name, Guid excludeProductId);
        Task<bool> ExistsBySlugAsync(string slug);
        Task<bool> ExistsBySkuAsync(string sku);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync();
    }
}