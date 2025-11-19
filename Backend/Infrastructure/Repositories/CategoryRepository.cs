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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly FoodyDbContext _context;
        public CategoryRepository(FoodyDbContext context) : base(context)
        {
            _context = context;
        } 

        public async Task<IEnumerable<Category>> GetCategoryTree(CancellationToken ct)
        {
            var query = await _context.Categories
                .Include(s => s.SubCategories)
                .ThenInclude(s => s.SubSubCategories)
                .ToListAsync(ct);

            return query;
        }

        public async Task<SubCategory> GetSubCategory(int id)
        {
            var query = await _context.SubCategories
                .FirstOrDefaultAsync(s => s.Id == id);
            return query;
        }

        public async Task<SubSubCategory> GetSubSubCategory(int id)
        {
            var query = await _context.SubSubCategories
                .FirstOrDefaultAsync(s => s.Id == id);
            return query;
        }


    }
}
