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

        public async Task<Order> GetUserOrder(Guid userId)
        {
            var query = await _context.Orders.FirstOrDefaultAsync(u => u.UserId == userId);
            return query;
        }
    }
}
