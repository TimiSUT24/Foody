using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Dto.Response
{
    public record CartTotals
    {
        public decimal Moms { get; init; }
        public decimal SubTotal { get; init; }
        public decimal Total { get; init; }

    }
}
