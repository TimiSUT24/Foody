using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Interfaces
{
    public interface IOrderService
    {
        Task<CreatedOrderResponse> CreateAsync(Guid userId, CreateOrderDto request, CancellationToken ct);
        Task<OrderResponse> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<UserOrderResponse>> GetUserOrders(Guid userId, OrderStatus? status, CancellationToken ct);
        Task<UserOrderResponse> GetUserOrder(Guid userId, Guid orderId, CancellationToken ct);
        Task<bool> CancelMyOrder(Guid userId, Guid orderId, CancellationToken ct);
        Task<CartTotals> CalculateTax(CartItemsDto cartItems, CancellationToken ct);
        Task<bool> UpdateOrder(UpdateOrder request, CancellationToken ct);


    }
}
