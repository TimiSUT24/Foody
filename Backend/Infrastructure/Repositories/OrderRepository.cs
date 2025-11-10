using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly FoodyDbContext _context;
        public OrderRepository(FoodyDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<List<Order>> GetMyOrders(Guid userId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(f => f.Food)
                    .Include(u => u.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(ct);
        }

        public async Task<Order> GetMyOrder(Guid userId, Guid orderId, CancellationToken ct)
        {
            var query = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Food)
                    .Include(u => u.User)
                    .OrderDescending()
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.Id == orderId, ct);

                    return query;
                   
        }

        public async Task<Order> GetOrder(Guid id, CancellationToken ct)
        {
            var query = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(f => f.Food)
                .FirstOrDefaultAsync(i => i.Id == id, ct);
            return query;
                
        }
    }
}
