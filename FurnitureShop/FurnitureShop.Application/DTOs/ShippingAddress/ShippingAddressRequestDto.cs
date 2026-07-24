using System.ComponentModel.DataAnnotations;
using FurnitureShop.Domain.Enums;

namespace FurnitureShop.Application.DTOs.ShippingAddress
{
    public class ShippingAddressRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^[6-9]\d{9}$",
            ErrorMessage = "Enter a valid Indian mobile number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; set; } = string.Empty;

        [StringLength(200)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = "India";

        [Required]
        [RegularExpression(
            @"^\d{6}$",
            ErrorMessage = "PIN code must contain exactly 6 digits.")]
        public string PinCode { get; set; } = string.Empty;

        [Required]
        public AddressType AddressType { get; set; }
    }
}