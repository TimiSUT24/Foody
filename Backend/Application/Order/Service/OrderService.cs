using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using Application.Order.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
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

        public async Task<bool> CreateAsync(Guid userId, CreateOrderDto request, CancellationToken ct)
        {
            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var product = await _uow.Products.GetByIdAsync<int>(item.FoodId, ct);
                if (product == null) throw new KeyNotFoundException("Cannot find product");
                if (product.Stock.Equals(0)) throw new ArgumentException("Product sold out");

                var orderItem = new OrderItem
                {
                    FoodId = item.FoodId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                };

                totalPrice += item.Quantity * product.Price;
                product.Stock -= item.Quantity;
                orderItems.Add(orderItem);
            }

            var order = new Domain.Models.Order
            {
                UserId = userId,
                TotalPrice = totalPrice,
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

            return true;
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var order = await _uow.Orders.GetOrder(id, ct);
            if(order == null)
            {
                throw new KeyNotFoundException($"No Order found");
            }
            var response = _mapper.Map<OrderResponse>(order);

            return response;
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
    }
}
