using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateAsync(Guid userId, CreateOrderDto request, CancellationToken ct);
        Task<Domain.Models.Order> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<UserOrderResponse>> GetUserOrders(Guid userId, CancellationToken ct);
        Task<UserOrderResponse> GetUserOrder(Guid userId, Guid orderId, CancellationToken ct);
        Task<bool> CancelMyOrder(Guid userId, Guid orderId, CancellationToken ct);
        Task UpdateStatusAsync(Guid orderId, string status, CancellationToken ct);


    }
}
