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
    public class ClassificationRepository : GenericRepository<Classification>, IClassificationRepository
    {
        private readonly FoodyDbContext _context;
        public ClassificationRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classification>> GetClassificationsByFoodIdAsync(int foodId, CancellationToken ct)
        {
            return await _context.Classifications
                                 .Where(i => i.FoodId == foodId)
                                 .ToListAsync(ct);
        }
    }
}
