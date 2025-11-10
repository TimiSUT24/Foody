using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record CreateOrderItemDto
    {
        public int FoodId { get; init; }
        public int Quantity { get; init; }
        
    }
}
