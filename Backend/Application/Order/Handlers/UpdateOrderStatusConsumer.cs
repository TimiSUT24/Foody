using Application.Order.Dto.Request;
using Application.Order.Events;
using Application.Order.Interfaces;
using Domain.Enum;
using Domain.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Handlers
{
    public class UpdateOrderStatusConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderStatusConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<ShipmentBookedEvent> context)
        {
            var msg = context.Message;
            var update = new UpdateOrder
            {
                Id = msg.Id,
                OrderStatus = OrderStatus.Pending.ToString(),
                PaymentMethod = msg.PaymentMethod,
                PaymentStatus = msg.PaymentStatus,
                ShippingInformation = new ShippingPatchDto
                {
                    ShipmentId = msg.ShippingInformation.ShipmentId,
                    TrackingId = msg.ShippingInformation.TrackingId,
                    TrackingUrl = msg.ShippingInformation.TrackingUrl,
                    Carrier = "Postnord"
                }

            };

            await _orderService.UpdateOrder(update, context.CancellationToken);
         
        }
    }
}
