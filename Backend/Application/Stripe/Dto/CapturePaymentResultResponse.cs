using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stripe.Dto
{
    public record CapturePaymentResultResponse
    {
        public string PaymentIntentId { get; init; } = null!;
        public string ChargeId { get; init; } = null!;
        public string Status { get; init; } = null!;
        public string PaymentMethod { get; init; } = null!;
        public long Amount { get; init; }
        public string Currency { get; init; } = null!;
    }
}
