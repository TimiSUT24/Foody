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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly FoodyDbContext _context;
        public ProductRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetProductDetailsById(int id, CancellationToken ct)
        {
            var query = await _context.Products
                .AsNoTracking()
                .Include(s => s.ProductAttributes)
                .Include(s => s.NutritionValues)
                .Include(s => s.Category).ThenInclude(s => s.SubCategories).ThenInclude(s => s.SubSubCategories)
                .FirstOrDefaultAsync(s => s.Id == id);

            return query; 
        }
    }
}
