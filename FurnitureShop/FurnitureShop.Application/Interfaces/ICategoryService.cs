using FurnitureShop.Application.DTOs.Product;
using FurnitureShop.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Guid> CreateAsync(CreateCategoryRequestDto request);
    }
}
