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
    public class NutritionValueRepository : GenericRepository<NutritionValue>, INutritionValueRepository
    {
        private readonly FoodyDbContext _context;
        public NutritionValueRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<NutritionValue>> GetNutritionValueByFoodIdAsync(int foodId, CancellationToken ct)
        {
            return await _context.NutritionValues
                                 .Where(nv => nv.FoodId == foodId)
                                 .ToListAsync(ct);
        }

        public async Task<NutritionValue> GetByName(string name, CancellationToken ct)
        {
            var query = await _context.NutritionValues.FirstOrDefaultAsync(rm => rm.Name == name, ct);
            return query;
        }
    }
}
