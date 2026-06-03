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
        [StringLength(
        50,
        MinimumLength = 2,
        ErrorMessage =
        "Category name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
