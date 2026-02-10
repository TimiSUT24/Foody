using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stripe.Dto
{
    public record PaymentIntentIdDto
    {
        public string PaymentIntentId { get; init; } = null!;
    }
}
