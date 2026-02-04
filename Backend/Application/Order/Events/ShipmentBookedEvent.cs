using Application.Order.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Events
{
    public record ShipmentBookedEvent
    {
        public Guid Id { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public ShippingPatchDto? ShippingInformation { get; set; }
    }  
}
