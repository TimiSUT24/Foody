using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record OrderItemsResponse(
        int FoodId,
        string FoodName,
        int Quantity,
        decimal UnitPrice
        );
   
}
