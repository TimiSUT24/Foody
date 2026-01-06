using Application.Order.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StripeChargeShippingOptions.Dto
{
    public record CreatePaymentRequestDto
    {
        public List<CartItemDto2> CartItems { get; init; } = new();
        public StripeShippingDto2 Shipping { get; init; } = null!;
        public decimal ShippingTax { get; init; }
        public string UserId { get; init; } = null!;
    }
}
