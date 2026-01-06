using Application.StripeChargeShippingOptions.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StripeChargeShippingOptions.Interfaces
{
    public interface IStripeService
    {
        Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentId);
        Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentId);
        Task<string> CreatePaymentIntentAsync(CreatePaymentRequestDto dto);
    }
}
