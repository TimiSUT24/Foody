using Application.Stripe.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stripe.Interfaces
{
    public interface IStripeService
    {
        Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentId);
        Task<CapturePaymentResultResponse> CapturePaymentIntentAsync(string paymentIntentId);
        Task<string> CreatePaymentIntentAsync(CreatePaymentRequestDto dto);
    }
}
