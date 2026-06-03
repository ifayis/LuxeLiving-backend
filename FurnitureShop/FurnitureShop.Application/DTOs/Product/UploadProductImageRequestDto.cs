using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Product
{
    public class UploadProductImageRequestDto
    {
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}