using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.ShippingAddress
{
    public class ShippingAddressRequestDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Phone number must be 10 digits.")]
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
        [RegularExpression(@"^[0-9]{6}$",
            ErrorMessage = "Pincode must be 6 digits.")]
        public string PinCode { get; set; } = string.Empty;
    }
}
