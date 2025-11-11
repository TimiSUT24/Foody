using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrder(Guid id, CancellationToken ct);
        Task<List<Order>> GetMyOrders(Guid userId, CancellationToken ct);
        Task<Order> GetMyOrder(Guid userId, Guid orderId, CancellationToken ct);
    }
}
