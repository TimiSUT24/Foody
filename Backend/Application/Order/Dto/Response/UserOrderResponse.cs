using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record UserOrderResponse
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public DateTime OrderDate { get; init; }
        public Decimal TotalPrice { get; init; }
        public string OrderStatus { get; init; } = string.Empty;
        public List<UserOrderItemsResponse> OrderItems { get; init; } = new();
    }
}
