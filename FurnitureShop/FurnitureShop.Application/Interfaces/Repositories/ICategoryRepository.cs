using FurnitureShop.Domain.Entities;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> GetBySlugAsync(string slug);
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetActiveAsync();
        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByNameAsync(
            string name,
            Guid excludeCategoryId);

        Task<bool> ExistsBySlugAsync(string slug);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task SaveChangesAsync();
    }
}