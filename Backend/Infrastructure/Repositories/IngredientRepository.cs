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
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        private readonly FoodyDbContext _context;
        public IngredientRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsByFoodIdAsync(int foodId, CancellationToken ct)
        {
            return await _context.Ingredients
                                 .Where(i => i.FoodId == foodId)
                                 .ToListAsync(ct);
        }
    }
}
