using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        #region Create
        Task AddAsync(Product product);
        #endregion
        #region Read
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
        #endregion
        #region Validation
        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByNameAsync(
            string name,
            Guid excludeProductId);

        Task<bool> ExistsBySlugAsync(string slug);
        Task<bool> ExistsBySkuAsync(string sku);
        #endregion
        #region Update
        Task UpdateAsync(Product product);
        #endregion
        #region Delete
        Task DeleteAsync(Product product);
        #endregion
        #region Save
        Task SaveChangesAsync();
        #endregion
    }
}