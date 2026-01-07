using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StripeChargeShippingOptions.Dto
{
    public record PaymentIntentIdDto
    {
        public string PaymentIntentId { get; init; } = null!;
    }
}
