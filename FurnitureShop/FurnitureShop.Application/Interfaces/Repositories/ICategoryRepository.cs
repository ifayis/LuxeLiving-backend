using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);

    }
}
