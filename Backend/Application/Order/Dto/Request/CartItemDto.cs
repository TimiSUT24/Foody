using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Request
{
    public record CartItemDto
    {
        public int Id { get; init; }
        public int Qty { get; init; }
    }
}
