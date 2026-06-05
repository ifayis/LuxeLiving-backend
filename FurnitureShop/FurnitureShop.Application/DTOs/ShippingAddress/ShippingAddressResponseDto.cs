using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.ShippingAddress
{
    public class ShippingAddressResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string PinCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
