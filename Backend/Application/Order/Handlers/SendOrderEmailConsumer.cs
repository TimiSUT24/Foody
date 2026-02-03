using Application.Order.Events;
using Application.Order.Interfaces;
using Domain.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Handlers
{
    public class SendOrderEmailConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _uow;

        public SendOrderEmailConsumer(IEmailService emailService, IUnitOfWork uow)
        {
            _emailService = emailService;
            _uow = uow;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = await _uow.Orders.GetOrder(context.Message.OrderId, context.CancellationToken);
            await _emailService.SendOrderConfirmationEmail(order.ShippingInformation.Email,order);
        }
    }
}
