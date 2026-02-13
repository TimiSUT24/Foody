using Application.Abstractions;
using Application.Order.Dto.Request;
using Application.Order.Events;
using Application.Order.Handlers;
using Application.Order.Service;
using Application.Postnord.Dto;
using Application.Postnord.Dto.Response;
using Application.Product.Dto.Response;
using Application.Product.Interfaces;
using Application.Stripe.Dto;
using Application.Stripe.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using ICSharpCode.SharpZipLib.Zip;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Unit
{
    public class OrderServiceTests
    {
        private readonly OrderServiceTestFixture _fixture;
        public OrderServiceTests()
        {
            _fixture = new OrderServiceTestFixture();    
        }


        [Fact]
        public async Task CreateAsync_Publishes_OrderCreatedEvent()
        {
            var ct = CancellationToken.None;

            //Arrange create order dto 
            var orderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        FoodId = _fixture.DefaultProduct.Id,
                        Quantity = 2
                    }
                },
                PaymentIntentId = "pi_test123",
                ServiceCode = "17",
                ShippingInformation = new Application.Order.Dto.Request.ShippingInformation
                {
                    FirstName = "Tim",
                    LastName = "Petersen",
                    Adress = "Street 1",
                    City = "Falkenberg",
                    State = "Halland",
                    PostalCode = "31173",
                    PhoneNumber = "123",
                    Email = "test@test.com"
                }
            };


            // ACT call order method
            await _fixture.Service.CreateAsync(Guid.NewGuid(), orderDto, ct);

            // ASSERT verify that event was published 
            _fixture.MockPublishEndpoint.Verify(p =>
                p.Publish(
                    It.Is<OrderCreatedEvent>(e =>
                        e.PaymentIntentId == "pi_test123" &&
                        e.PaymentStatus == "succeeded"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Fact]
        public async Task Consume_OrderCreatedEvent_Publishes_ShipmentBookedEvent()
        {
            // Arrange
            // Arrange
            var orderEvent = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                PaymentIntentId = "pi_test",
                PaymentStatus = "succeeded",
                PaymentMethod = "card",
                TotalWeight = 2.5m,
                Shipping = new ShippingDto
                {
                    Email = "test@test.com",
                    Lastname = "Petersen",
                    ServiceCode = "POSTNORD",
                    Shipping = new StripeShippingDto
                    {
                        Name = "Tim",
                        Phone = "123",
                        Address = new AddressDto
                        {
                            Line1 = "Street 1",
                            City = "Falkenberg",
                            Postal_Code = "31173"
                        }
                    }
                }
            };

            var bookingResponse = new PostNordBookingResponseDto
            {
                BookingId = "BOOK123",
                IdInformation = new IdInformationDto[]
                {
                    new IdInformationDto
                    {
                        Ids = new IdsDto[]
                        {                        
                            new IdsDto { Value = "TRACK123" }
                        },
                        Urls = new UrlsDto[]
                        {
                            new UrlsDto { Type = "TRACKING", Url = "http://tracking.url" }
                        }
                    }
                }
            };

            _fixture.MockPostNord.Setup(s => s.BookShipmentAsync(It.IsAny<PostNordBookingRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookingResponse);

            var contextMock = new Mock<ConsumeContext<OrderCreatedEvent>>();
            contextMock.SetupGet(x => x.Message).Returns(orderEvent);
            contextMock.SetupGet(x => x.CancellationToken).Returns(CancellationToken.None);

            // Act
            await _fixture.Consumer.Consume(contextMock.Object);

            _fixture.MockPostNord.Verify(x => x.BookShipmentAsync(
                It.Is<PostNordBookingRequestDto>(s => s.OrderId == orderEvent.OrderId.ToString()),
                It.IsAny<CancellationToken>()),
                Times.Once);



            // Assert that ShipmentBookedEvent was published
            _fixture.MockPublishEndpoint.Verify(p =>
                p.Publish(
                    It.Is<ShipmentBookedEvent>(e =>
                        e.Id == orderEvent.OrderId &&
                        e.PaymentStatus == orderEvent.PaymentStatus &&
                        e.ShippingInformation.ShipmentId == bookingResponse.BookingId &&
                        e.ShippingInformation.TrackingId == "TRACK123" &&
                        e.ShippingInformation.TrackingUrl == "http://tracking.url"
                    ),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Consume_ShipmentBookedEvent_CheckIfEmailWasSent()
        {

            //Arrange Order 
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ShippingInformation = new Domain.Models.ShippingInformation
                {
                    Email = "test@test.com"
                }
            };

            //Arrange ShipmentBookedEvent data 
            var shipmentEvent = new ShipmentBookedEvent
            {
                Id = order.Id,
                ShippingInformation = new ShippingPatchDto
                {
                    Email = order.ShippingInformation.Email
                }
            };

            //Mock emailService and GetOrder
            _fixture.MockEmail.Setup(s => s.SendOrderConfirmationEmail(It.IsAny<string>(), It.IsAny<Order>()));
            _fixture.MockUow.Setup(s => s.Orders.GetOrder(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
              

            var contextMock = new Mock<ConsumeContext<ShipmentBookedEvent>>();
            contextMock.SetupGet(s => s.Message).Returns(shipmentEvent);
            contextMock.SetupGet(s => s.CancellationToken).Returns(CancellationToken.None);

            //Act Consume SendOrderEmailConsumer
            await _fixture.EmailConsumer.Consume(contextMock.Object);

            //Assert verify method and sends the right data
            _fixture.MockEmail.Verify(e => e.SendOrderConfirmationEmail(
            shipmentEvent.ShippingInformation.Email,
            It.Is<Order>(s => s.Id == order.Id && s.ShippingInformation.Email == order.ShippingInformation.Email)),
            Times.Once);

        }

        [Fact]
        public async Task Consume_ShipmentBookedEvent_CheckIfOrderWasUpdated()
        {
            var shipmentEvent = new ShipmentBookedEvent
            {
                Id = Guid.NewGuid(),
                OrderStatus = "Processing",
                PaymentStatus = "Paid",
                PaymentMethod = "klarna",
                ShippingInformation = new ShippingPatchDto
                {
                    TrackingId = "sda2823233j32"
                }
            };

            _fixture.MockOrderService.Setup(s => s.UpdateOrder(It.IsAny<UpdateOrder>(), It.IsAny<CancellationToken>()));

            var contextMock = new Mock<ConsumeContext<ShipmentBookedEvent>>();
            contextMock.SetupGet(s => s.Message).Returns(shipmentEvent);
            contextMock.SetupGet(s => s.CancellationToken).Returns(CancellationToken.None);

            await _fixture.UpdateConsumer.Consume(contextMock.Object);

            _fixture.MockOrderService.Verify(s => s.UpdateOrder(
                It.Is<UpdateOrder>(s => s.Id == shipmentEvent.Id &&
                s.OrderStatus == shipmentEvent.OrderStatus &&
                s.PaymentStatus == shipmentEvent.PaymentStatus &&
                s.PaymentMethod == shipmentEvent.PaymentMethod &&
                s.ShippingInformation!.TrackingId == shipmentEvent.ShippingInformation.TrackingId),
                It.IsAny<CancellationToken>()),
                Times.Once);


        } 
    }
}
