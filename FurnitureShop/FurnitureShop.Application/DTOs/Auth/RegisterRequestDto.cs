using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z\s]+$",
            ErrorMessage = "Full name must contain only letters and spaces.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^\S+@\S+\.\S+$",
            ErrorMessage = "Email must not contain spaces or invalid characters.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
