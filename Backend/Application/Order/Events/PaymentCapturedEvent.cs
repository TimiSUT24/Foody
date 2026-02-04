using Application.Postnord.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Events
{
    public class PaymentCapturedEvent
    {
        public Guid OrderId { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalWeight { get; set; }
        public ShippingDto? Shipping { get; set; }
    }
}
