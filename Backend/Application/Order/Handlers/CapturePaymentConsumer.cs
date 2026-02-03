using Application.Order.Events;
using Application.StripeChargeShippingOptions.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Handlers
{
    public class CapturePaymentConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IStripeService _stripeService;

        public CapturePaymentConsumer(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            await _stripeService.CapturePaymentIntentAsync(context.Message.PaymentIntentId);
        }
    }
}
