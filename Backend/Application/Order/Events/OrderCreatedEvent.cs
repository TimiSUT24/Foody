using Application.Postnord.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Events
{
    public record OrderCreatedEvent(
        Guid OrderId,
        string PaymentIntentId,
        decimal TotalWeight,
        ShippingDto Shipping
        );  
}
