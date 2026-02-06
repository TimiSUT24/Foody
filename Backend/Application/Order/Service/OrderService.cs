using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using Application.Order.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Application.Abstractions;
using Microsoft.Extensions.Options;
using Application.Product.Interfaces;
using MassTransit;
using Application.Order.Events;
using Application.Postnord.Dto;
using Application.StripeChargeShippingOptions.Interfaces;

namespace Application.Order.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IStripeService _stripeService;
        private readonly ICalculateDiscount _discount;
        private readonly ICacheService _cache;
        private readonly CacheSettings _cacheSettings;
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(IUnitOfWork uow, IMapper mapper,IStripeService stripeService,IPublishEndpoint publishEndpoint,IProductService productService, IEmailService emailService, ICalculateDiscount discount, ICacheService cache, IOptions<CacheSettings> cacheSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _stripeService = stripeService;
            _discount = discount;
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        private decimal ConvertToKg(decimal? value, string? unit)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Weight value is required");

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentNullException(nameof(unit), "Weight unit is required");

            return unit.ToLower() switch
            {
                "kg" => value.Value,
                "g" => value.Value / 1000m,
                "l" => value.Value, // 1 liter ≈ 1 kg
                _ => throw new ArgumentException($"Unsupported weight unit: {unit}")
            };
        }

        public async Task<CreatedOrderResponse> CreateAsync(Guid userId, CreateOrderDto request, CancellationToken ct)
        {
            try
            {
                var fetchedProducts = new List<Domain.Models.Product>();
                foreach (var item in request.Items)
                {                   
                    var product = await _uow.Products.GetByIdAsync(item.FoodId,ct);
                    if(product == null)
                    {
                        throw new KeyNotFoundException("Invalid product ID");
                    }
                    fetchedProducts.Add(product);
                }
                var orderItems = new List<OrderItem>();

                var cartItems = new CartItemsDto
                {
                    Items = request.Items.Select(i => new CartItemDto
                    {
                        Id = i.FoodId,
                        Qty = i.Quantity
                    }).ToList(),
                    ServiceCode = request.ServiceCode,
                };
                var totals = await CalculateTax(cartItems, ct);
                decimal? totalWeightKg = 0m;

                foreach (var item in request.Items)
                {
                    var product = fetchedProducts.First(s => s.Id == item.FoodId);
                    if (product == null) throw new KeyNotFoundException("Cannot find product");
                    if (product.Stock <= 0) throw new ArgumentException("Product sold out");
                    if (item.Quantity > product.Stock) throw new InvalidOperationException("Quantity exceeds product stock");

                    var unitToKg = ConvertToKg(product.WeightValue, product.WeightUnit);
                    totalWeightKg += unitToKg * item.Quantity;

                    var orderItem = new OrderItem
                    {
                        FoodId = item.FoodId,
                        Quantity = item.Quantity,
                        UnitPrice = totals.UnitPrice,
                        UnitPriceOriginal = product.Price,
                        WeightValue = (decimal)product.WeightValue
                    };
                    product.Stock -= item.Quantity;
                    orderItems.Add(orderItem);
                }

                var order = new Domain.Models.Order
                {
                    UserId = userId,
                    TotalPrice = totals.Total,
                    Moms = totals.Moms,
                    SubTotal = totals.SubTotal,
                    OrderStatus = Domain.Enum.OrderStatus.Pending,
                    TotalWeight = (decimal)totalWeightKg,
                    ShippingTax = totals.ShippingTax,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = orderItems,
                    ShippingInformation = new Domain.Models.ShippingInformation
                    {
                        FirstName = request.ShippingInformation.FirstName,
                        LastName = request.ShippingInformation.LastName,
                        Adress = request.ShippingInformation.Adress,
                        City = request.ShippingInformation.City,
                        State = request.ShippingInformation.State,
                        PostalCode = request.ShippingInformation.PostalCode,
                        PhoneNumber = request.ShippingInformation.PhoneNumber,
                        Email = request.ShippingInformation.Email
                    }
                };


                await _uow.Orders.AddAsync(order, ct);
                await _uow.SaveChangesAsync(ct);

                var paymentResult = await _stripeService.CapturePaymentIntentAsync(request.PaymentIntentId);

                await _publishEndpoint.Publish(new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    PaymentIntentId = request.PaymentIntentId,
                    TotalWeight = order.TotalWeight,
                    PaymentMethod = paymentResult.PaymentMethod,
                    PaymentStatus = paymentResult.Status,
                    Shipping = new ShippingDto
                    {
                        ServiceCode = request.ServiceCode,
                        Email = request.ShippingInformation.Email,
                        Lastname = request.ShippingInformation.LastName,
                        Shipping = new StripeShippingDto
                        {
                            Phone = request.ShippingInformation.PhoneNumber,
                            Name = request.ShippingInformation.FirstName,
                            Address = new AddressDto
                            {
                                Line1 = request.ShippingInformation.Adress,
                                Postal_Code = request.ShippingInformation.PostalCode,
                                City = request.ShippingInformation.City
                            }
                        }

                    }
                },
                    ct);

                return new CreatedOrderResponse
                {
                    Status = paymentResult.Status
                };
            }
            catch (Exception ex)
            {
                var paymentResult = await _stripeService.CancelPaymentIntentAsync(request.PaymentIntentId);

                return new CreatedOrderResponse
                {
                    Status = paymentResult.Status,
                    Message = $"Order creation failed {ex.Message}"
                };
            }
           
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var order = await _uow.Orders.GetOrder(id, ct);
            if(order == null)
            {
                throw new KeyNotFoundException($"No Order found");
            }
            var mapping = _mapper.Map<OrderResponse>(order);
            return mapping;
        }

        public async Task<List<UserOrderResponse>> GetUserOrders(Guid userId, OrderStatus? status, CancellationToken ct)
        {
            var cacheKey = $"orders:{userId}:{status}";

            return await _cache.GetOrCreateAsync("user:",cacheKey, async _ =>
            {
                var userOrders = await _uow.Orders.GetMyOrders(userId, status, ct);
                if (userOrders == null)
                {
                    throw new KeyNotFoundException("User has no orders");
                }

                var response = _mapper.Map<List<UserOrderResponse>>(userOrders);

                return response;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );         
        }

        public async Task<UserOrderResponse> GetUserOrder(Guid userId, Guid orderId, CancellationToken ct)
        {
            var cacheKey = $"order:{userId}:{orderId}";

            return await _cache.GetOrCreateAsync("user:",cacheKey, async _ =>
            {
                var userOrder = await _uow.Orders.GetMyOrder(userId, orderId, ct);
                if (userOrder == null) throw new KeyNotFoundException("Order was not found");

                var response = _mapper.Map<UserOrderResponse>(userOrder);

                return response;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
            
        }

        public async Task<bool> UpdateOrder(UpdateOrder request, CancellationToken ct) 
        {
            var order = await _uow.Orders.GetOrder(request.Id, ct);
            if (order == null)
            {
                throw new KeyNotFoundException("order not found");
            }
            bool updated = false;

            if (!string.IsNullOrEmpty(request.OrderStatus))
            {
                if (Enum.TryParse<Domain.Enum.OrderStatus>(request.OrderStatus, true, out var os))
                {
                    order.OrderStatus = os;
                    updated = true;
                }
                
            }
            if (!string.IsNullOrEmpty(request.PaymentStatus))
            {
                if (request.PaymentStatus.Equals("succeeded", StringComparison.OrdinalIgnoreCase))
                {
                    order.PaymentStatus = Domain.Enum.PaymentStatus.Paid;
                    updated = true;
                }
                else
                {
                    order.PaymentStatus = Domain.Enum.PaymentStatus.Failed;
                    updated = true;
                }
                   
            }

            if (!string.IsNullOrEmpty(request.PaymentMethod))
            {
                order.PaymentMethod = request.PaymentMethod;
                updated = true;
            }

            if (request.ShippingInformation != null)
            {
                var s = order.ShippingInformation;

                s.ShipmentId = request.ShippingInformation.ShipmentId ?? s.ShipmentId;
                s.TrackingId = request.ShippingInformation.TrackingId ?? s.TrackingId;
                s.TrackingUrl = request.ShippingInformation.TrackingUrl ?? s.TrackingUrl;
                s.Carrier = request.ShippingInformation.Carrier ?? s.Carrier;

                s.FirstName = request.ShippingInformation.FirstName ?? s.FirstName;
                s.LastName = request.ShippingInformation.LastName ?? s.LastName;
                s.Email = request.ShippingInformation.Email ?? s.Email;
                s.PhoneNumber = request.ShippingInformation.PhoneNumber ?? s.PhoneNumber;
                s.Adress = request.ShippingInformation.Adress ?? s.Adress;
                s.City = request.ShippingInformation.City ?? s.City;
                s.State = request.ShippingInformation.State ?? s.State;
                s.PostalCode = request.ShippingInformation.PostalCode ?? s.PostalCode;

            }

            if (!updated) { return false; }

            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("user:");

            return true;
        }

        public async Task<bool> CancelMyOrder(Guid userId, Guid orderId, CancellationToken ct)
        {
            var userOrder = await _uow.Orders.GetMyOrder(userId, orderId, ct);
            if (userOrder == null) throw new KeyNotFoundException("Order was not found");

            var hoursSinceOrder = DateTime.UtcNow - userOrder.OrderDate;

            if (userOrder.OrderStatus == Domain.Enum.OrderStatus.Pending)
            {
                if (hoursSinceOrder.TotalHours <= 12)
                {
                    userOrder.OrderStatus = Domain.Enum.OrderStatus.Cancelled;
                    _uow.Orders.Update(userOrder);
                    await _uow.SaveChangesAsync(ct);
                    await _cache.RemoveByPrefixAsync("user:");
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("You can no longer cancel this order (12-hour limit passed).");
                }
            }
            else
            {
                throw new InvalidOperationException("Only pending orders can be canceled.");
            }

        }

        public async Task<CartTotals> CalculateTax(CartItemsDto cartItems, CancellationToken ct)
        {
            decimal subTotal = 0M;
            decimal momsTotal = 0M;
            decimal momsRate = 0.12M;
            decimal shippingTax = 0M;
            decimal total = 0M;
            var unitPrice = 0M;
            var productIds = cartItems.Items.Select(s => s.Id).ToList();

            var products = await _productService.GetByIdsAsync(productIds, ct);
            var now = DateTime.UtcNow;

            foreach (var item in cartItems.Items)
            {

                var product = products.FirstOrDefault(s => s.Id == item.Id);
                if (product == null) throw new KeyNotFoundException($"Product id {product.Id} not found");

                unitPrice = _discount.GetFinalPrice(product, now);
                subTotal += unitPrice * item.Qty;                
          
            }

            shippingTax = GetShippingTax(cartItems.ServiceCode);
            if (subTotal >= 300M)
            {
                shippingTax = 0M;
            }
            momsTotal = (subTotal + shippingTax) * momsRate;
            total = subTotal + momsTotal + shippingTax;

            return new CartTotals
            {
                SubTotal = decimal.Round(subTotal,2,MidpointRounding.AwayFromZero),
                Moms = decimal.Round(momsTotal, 2, MidpointRounding.AwayFromZero),
                Total = decimal.Round(total,2,MidpointRounding.AwayFromZero),
                ShippingTax = shippingTax,
                UnitPrice = unitPrice
            };
        }


        private decimal GetShippingTax(string? serviceCode)
        {
            return serviceCode switch
            {
                "17" => 39M,
                "" => 0M
            };
        }

    }
}
