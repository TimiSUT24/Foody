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
    public class RawMaterialRepository : GenericRepository<RawMaterial>, IRawMaterialRepository
    {
        private readonly FoodyDbContext _context;
        public RawMaterialRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RawMaterial>> GetRawMaterialsByFoodIdAsync(int foodId, CancellationToken ct)
        {
            return await _context.RawMaterials
                                 .Where(rm => rm.FoodId == foodId)
                                 .ToListAsync(ct);
        }
    }
}
