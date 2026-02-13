using Application.Postnord.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public decimal TotalWeight { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public ShippingDto? Shipping { get; set; }
    }
}
