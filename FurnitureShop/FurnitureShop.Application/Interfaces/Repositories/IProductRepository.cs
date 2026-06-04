using FurnitureShop.Domain.Enitities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task<Product?> GetByIdAsync(Guid id);

        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetByCategoryAsync(Guid categoryId);

        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByNameExceptIdAsync(
            string name,
            Guid productId);

        Task DeleteAsync(Product product);

        Task DeleteAllAsync();

        Task SaveChangesAsync();
    }
}