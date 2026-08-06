using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Auth
{
    public class VerifyEmailRequestDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}