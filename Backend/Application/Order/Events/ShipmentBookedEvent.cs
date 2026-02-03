using Application.Order.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Events
{
    public record ShipmentBookedEvent(
        Guid Id,
        string? PaymentStatus,
        string? OrderStatus,
        string? PaymentMethod,
        ShippingPatchDto? ShippingInformation
        );
   
}
