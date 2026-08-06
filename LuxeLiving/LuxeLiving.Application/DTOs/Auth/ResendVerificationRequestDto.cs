using System.ComponentModel.DataAnnotations;

namespace FurnitureShop.Application.DTOs.Auth
{
    public class ResendVerificationRequestDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
    }
}