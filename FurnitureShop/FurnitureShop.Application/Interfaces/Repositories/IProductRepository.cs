using FurnitureShop.Application.Common;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetByCategoryAsync(Guid categoryId);
        Task<bool> ExistsByNameAsync(string name);
        Task DeleteAsync(Product product);
        Task DeleteAllAsync();
        Task SaveChangesAsync();
    }
}
