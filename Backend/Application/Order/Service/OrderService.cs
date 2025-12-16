using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using Application.Order.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
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
            var products = await _uow.Products.GetAllAsync(ct);
            foreach(var item in request.Items)
            {
                if (!products.Any(s => s.Id == item.FoodId))
                {
                    throw new KeyNotFoundException("Invalid id");
                }
            }
            var orderItems = new List<OrderItem>();

            var cartItems = new CartItemsDto
            {
                Items = request.Items.Select(i => new CartItemDto
                {
                    Id = i.FoodId,
                    Qty = i.Quantity
                }).ToList()
            };
            var totals = await CalculateTax(cartItems, ct);
            decimal? totalWeightKg = 0m;

            foreach (var item in request.Items)
            {
                var product = await _uow.Products.GetByIdAsync<int>(item.FoodId, ct);
                if (product == null) throw new KeyNotFoundException("Cannot find product");
                if (product.Stock <= 0) throw new ArgumentException("Product sold out");
                if (item.Quantity > product.Stock) throw new InvalidOperationException("Quantity exceeds product stock");

                var unitToKg = ConvertToKg(product.WeightValue, product.WeightUnit);
                totalWeightKg += unitToKg * item.Quantity;

                var orderItem = new OrderItem
                {
                    FoodId = item.FoodId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
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
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems,
                ShippingInformation = new Domain.Models.ShippingInformation
                {
                    FirstName = request.ShippingInformation.FirstName,
                    LastName = request.ShippingInformation.LastName,
                    Adress = request.ShippingInformation.Adress,
                    City = request.ShippingInformation.City,
                    State= request.ShippingInformation.State,
                    PostalCode = request.ShippingInformation.PostalCode,
                    PhoneNumber = request.ShippingInformation.PhoneNumber,
                    Email = request.ShippingInformation.Email
                }
            };

         
            await _uow.Orders.AddAsync(order, ct);
            await _uow.SaveChangesAsync(ct);

            return new CreatedOrderResponse
            {
                OrderId = order.Id,
                TotalWeightKg = totalWeightKg
            };
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

        public async Task<List<UserOrderResponse>> GetUserOrders(Guid userId, CancellationToken ct)
        {
            var userOrders = await _uow.Orders.GetMyOrders(userId, ct);
            if (userOrders == null)
            {
                throw new KeyNotFoundException("User has no orders");
            }

            var response = _mapper.Map<List<UserOrderResponse>>(userOrders);

            return response;

        }

        public async Task<UserOrderResponse> GetUserOrder(Guid userId, Guid orderId, CancellationToken ct)
        {
            var userOrder = await _uow.Orders.GetMyOrder(userId, orderId, ct);
            if (userOrder == null) throw new KeyNotFoundException("Order was not found");

            var response = _mapper.Map<UserOrderResponse>(userOrder);

            return response;
        }

        public async Task<bool> UpdateOrderStatus(UpdateOrderStatus request, CancellationToken ct) 
        {
            var order = await _uow.Orders.GetByIdAsync<Guid>(request.Id, ct);
            if (order == null)
            {
                throw new KeyNotFoundException("order not found");
            }
            if (!string.IsNullOrEmpty(request.OrderStatus))
            {
                if (Enum.TryParse<Domain.Enum.OrderStatus>(request.OrderStatus, true, out var os))
                    order.OrderStatus = os;
            }
            if (!string.IsNullOrEmpty(request.PaymentStatus))
            {
                if (request.PaymentStatus.Equals("succeeded", StringComparison.OrdinalIgnoreCase))
                {
                    order.PaymentStatus = Domain.Enum.PaymentStatus.Paid;
                }
                else
                {
                    order.PaymentStatus = Domain.Enum.PaymentStatus.Failed;
                }
                   
            }

            await _uow.SaveChangesAsync(ct);

            var updatedOrder = _uow.Orders.GetByIdAsync<Guid>(request.Id, ct);
            if(updatedOrder.Result.OrderStatus.ToString() == request.OrderStatus || updatedOrder.Result.PaymentStatus.ToString() == request.PaymentStatus)
            {
                return true;
            }
            return false;
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
            decimal subTotal = 0;
            decimal momsTotal = 0;
            decimal momsRate = 0.12M;
            decimal total = 0;
            var products = await _uow.Products.GetAllAsync(ct);
            foreach (var item in cartItems.Items)
            {

                if (!products.Any(s => s.Id == item.Id))
                {
                    throw new KeyNotFoundException("Invalid id");
                }
                
                
                var product = await _uow.Products.GetByIdAsync(item.Id, ct);

                var itemSubTotal = product.Price * item.Qty;
                var lineMoms = itemSubTotal * momsRate;

                subTotal += itemSubTotal;
                momsTotal += lineMoms;
                total = subTotal + momsTotal;

            }

            return new CartTotals
            {
                SubTotal = decimal.Round(subTotal,2,MidpointRounding.AwayFromZero),
                Moms = decimal.Round(momsTotal, 2, MidpointRounding.AwayFromZero),
                Total = decimal.Round(total,2,MidpointRounding.AwayFromZero),
            };
        }

      

    }
}
