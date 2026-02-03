using FurnitureShop.Domain.Enitities;
using System;

namespace FurnitureShop.Domain.Entities
{
    public class ShippingAddress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
