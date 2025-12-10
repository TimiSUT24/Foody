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
        public decimal TotalPrice { get; init; }
        public decimal SubTotal { get; init; }
        public decimal Moms { get; init; } 
        public decimal ShippingTax { get; init; } 
        public string OrderStatus { get; init; } = string.Empty;
        public string PaymentStatus { get; init; } = string.Empty;
        public List<UserOrderItemsResponse> OrderItems { get; init; } = new();
        public UserOrderShippingResponse ShippingItems { get; init; } = new();
    }
}
