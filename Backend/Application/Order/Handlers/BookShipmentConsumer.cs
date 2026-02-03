using Application.Order.Dto.Request;
using Application.Order.Events;
using Application.Order.Interfaces;
using Application.Postnord.Dto;
using Application.Postnord.Interfaces;
using Domain.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Handlers
{
    public class BookShipmentConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPostnordService _postnord;
        private readonly IPublishEndpoint _publishEndpoint;

        public BookShipmentConsumer(IUnitOfWork uow, IPublishEndpoint publishEndpoint, IPostnordService postnord)
        {
            _uow = uow;
            _postnord = postnord;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var evt = context.Message;
            var request = new PostNordBookingRequestDto
            {
                OrderId = evt.OrderId.ToString(),
                TotalWeight = evt.TotalWeight,
                Shipping = new ShippingDto
                {
                    ServiceCode = evt.Shipping.ServiceCode,
                    Email = evt.Shipping.Email,
                    Lastname = evt.Shipping.Lastname,
                    Shipping = new StripeShippingDto
                    {                       
                        Name = evt.Shipping.Shipping.Name,
                        Phone = evt.Shipping.Shipping.Phone,
                        Address = new AddressDto
                        {
                            Line1 = evt.Shipping.Shipping.Address.Line1,
                            City = evt.Shipping.Shipping.Address.City,
                            Postal_Code = evt.Shipping.Shipping.Address.Postal_Code
                        }
                    }
                }

            };
            var booking = await _postnord.BookShipmentAsync(request,context.CancellationToken);

            await _publishEndpoint.Publish(new ShipmentBookedEvent(
                evt.OrderId,
                
                ));
        }
    }
}
