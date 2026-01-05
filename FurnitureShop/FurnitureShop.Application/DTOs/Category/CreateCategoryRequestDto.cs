using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
