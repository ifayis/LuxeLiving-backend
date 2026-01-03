using FurnitureShop.Domain.Enitities;
using FurnitureShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<List<Category>> GetAllAsync();
        Task DeleteAsync(Category category);
        Task DeleteAllAsync();
        Task SaveChangesAsync();
    }
}
