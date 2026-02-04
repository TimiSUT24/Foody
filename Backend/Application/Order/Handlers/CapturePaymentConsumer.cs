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
        private readonly IPublishEndpoint _publishEndpoint;

        public CapturePaymentConsumer(IStripeService stripeService, IPublishEndpoint publishEndpoint)
        {
            _stripeService = stripeService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var result = await _stripeService.CapturePaymentIntentAsync(context.Message.PaymentIntentId);

            await _publishEndpoint.Publish(new PaymentCapturedEvent{
                OrderId = context.Message.OrderId,
                PaymentMethod = result.PaymentMethod,
                PaymentStatus = result.Status,
                TotalWeight = context.Message.TotalWeight,
                Shipping = context.Message.Shipping
            });
        }
    }
}
