using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stripe.Dto
{
    public record CartItemDto2
    {
        public decimal Price { get; init; }
        public int Qty { get; init; }
        public string Name { get; init; } = null!;
    }
}
