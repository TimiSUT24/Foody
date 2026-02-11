using Application.Abstractions;
using Application.Order.Service;
using Application.Product.Interfaces;
using Application.Stripe.Interfaces;
using AutoMapper;
using Domain.Interfaces;
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
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IStripeService> _mockStripeService;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICalculateDiscount> _mockDiscount;
        private readonly Mock<ICacheService> _mockCache;
        private readonly IOptions<CacheSettings> _cacheOptions;
        private readonly OrderService _orderService;


        public OrderServiceTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockStripeService = new Mock<IStripeService>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
            _mockMapper = new Mock<IMapper>();
            _mockDiscount = new Mock<ICalculateDiscount>();
            _mockCache = new Mock<ICacheService>();
            _mockUow = new Mock<IUnitOfWork>();
            _cacheOptions = Options.Create(new CacheSettings());



            _orderService = new OrderService(
                _mockUow.Object,
                _mockMapper.Object,
                _mockStripeService.Object,
                _mockPublishEndpoint.Object,
                _mockProductService.Object,
                _mockDiscount.Object,
                _mockCache.Object,
                _cacheOptions 
            );
        }
    }
}
