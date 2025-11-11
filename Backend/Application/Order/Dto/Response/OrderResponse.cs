using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record OrderResponse(
        Guid Id,
        Guid UserId,
        DateTime OrderDate,
        Decimal TotalPrice,
        string OrderStatus,
        List<OrderItemsResponse> OrderItems 
        );

 
}
