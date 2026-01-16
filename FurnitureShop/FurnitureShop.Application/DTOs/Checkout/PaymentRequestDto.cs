using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Application.DTOs.Checkout
{
    public class PaymentRequestDto
    {
        [Required]
        public string PaymentMethod { get; set; }
    }
}
