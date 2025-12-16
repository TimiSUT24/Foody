using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record CreatedOrderResponse
    {
        public Guid OrderId { get; init; }
        public decimal? TotalWeightKg { get; init; }
    }
}
