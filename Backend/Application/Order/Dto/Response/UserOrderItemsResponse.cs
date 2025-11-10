using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record UserOrderItemsResponse
    {
        public required string FoodName { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
        public string DiscountInfo { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
        public string Supplier { get; init; } = string.Empty;
    }
    
}
