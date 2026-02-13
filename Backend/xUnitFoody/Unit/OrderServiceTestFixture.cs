using Application.Abstractions;
using Application.Order.Handlers;
using Application.Order.Interfaces;
using Application.Order.Service;
using Application.Postnord.Dto;
using Application.Postnord.Dto.Response;
using Application.Postnord.Interfaces;
using Application.Product.Interfaces;
using Application.Stripe.Dto;
using Application.Stripe.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Unit
{
    public class OrderServiceTestFixture
    {
        public Mock<IOrderService> MockOrderService { get; private set; }
        public Mock<IProductService> MockProductService { get; private set; }
        public Mock<IStripeService> MockStripeService { get; private set; }
        public Mock<IPublishEndpoint> MockPublishEndpoint { get; private set; }
        public Mock<IUnitOfWork> MockUow { get; private set; }
        public Mock<IOrderRepository> MockOrderRepo { get; private set; }
        public Mock<ICalculateDiscount> MockDiscount { get; private set; }
        public Mock<IMapper> MockMapper { get; private set; }
        public Mock<ICacheService> MockCache { get; private set; }
        public Mock<IProductRepository> MockProductRepo { get; private set; }
        public IOptions<CacheSettings> CacheOptions { get; private set; }
        public OrderService Service { get; private set; }

        public Product DefaultProduct { get; private set; }

        public Mock<IPostnordService> MockPostNord { get; private set; }
        public BookShipmentConsumer Consumer {get; private set;}

        public Mock<IEmailService> MockEmail { get; private set; }
        public SendOrderEmailConsumer EmailConsumer { get; private set; }
        public UpdateOrderStatusConsumer UpdateConsumer { get; private set; }

        public OrderServiceTestFixture()
        {
            MockProductService = new Mock<IProductService>();
            MockStripeService = new Mock<IStripeService>();
            MockPublishEndpoint = new Mock<IPublishEndpoint>();
            MockUow = new Mock<IUnitOfWork>();
            MockOrderRepo = new Mock<IOrderRepository>();
            MockDiscount = new Mock<ICalculateDiscount>();
            MockCache = new Mock<ICacheService>();
            MockMapper = new Mock<IMapper>();
            MockProductRepo = new Mock<IProductRepository>();
            CacheOptions = Options.Create(new CacheSettings());
            MockPostNord = new Mock<IPostnordService>();
            MockEmail = new Mock<IEmailService>();
            MockOrderService = new Mock<IOrderService>();

            // default product
            DefaultProduct = new Product
            {
                Id = 1,
                Price = 100,
                Stock = 10,
                WeightValue = 1,
                WeightUnit = "kg"
            };

            // default setups
            MockUow.Setup(x => x.Orders).Returns(MockOrderRepo.Object);
            MockUow.Setup(x => x.Products).Returns(MockProductRepo.Object);
            MockOrderRepo.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);
            MockProductRepo.Setup(r => r.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(DefaultProduct);
            MockUow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            MockProductService.Setup(p => p.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Product> { DefaultProduct });
            MockDiscount.Setup(d => d.GetFinalPrice(DefaultProduct, It.IsAny<DateTime>()))
                        .Returns(DefaultProduct.Price);
            MockStripeService.Setup(s => s.CapturePaymentIntentAsync(It.IsAny<string>()))
                             .ReturnsAsync(new CapturePaymentResultResponse
                             {
                                 Status = "succeeded",
                                 PaymentMethod = "card"
                             });

            // service under test
            Service = new OrderService(
                MockUow.Object,
                MockMapper.Object,
                MockStripeService.Object,
                MockPublishEndpoint.Object,
                MockProductService.Object,
                MockDiscount.Object,
                MockCache.Object,
                CacheOptions
            );

            Consumer = new BookShipmentConsumer(
                MockPublishEndpoint.Object,
                MockPostNord.Object
                );

            EmailConsumer = new SendOrderEmailConsumer(
                MockEmail.Object,
                MockUow.Object
                );

            UpdateConsumer = new UpdateOrderStatusConsumer(
                MockOrderService.Object
                );
        }
    }
}
