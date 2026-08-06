using System.ComponentModel.DataAnnotations;

namespace LuxeLiving.Application.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
