using FurnitureShop.Application.DTOs.Category;
using FurnitureShop.Application.Interfaces.Repositories;
using FurnitureShop.Application.Interfaces.Services;
using FurnitureShop.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> CreateAsync(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _categoryRepository.AddAsync(category);
            return category.Id;
        }
    }
}
