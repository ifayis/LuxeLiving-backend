using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
