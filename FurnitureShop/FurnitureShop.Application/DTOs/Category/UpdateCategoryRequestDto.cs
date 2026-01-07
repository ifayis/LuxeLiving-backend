using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Category
{
    public class UpdateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
