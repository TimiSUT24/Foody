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
                .FirstOrDefaultAsync(s => s.Id == id, ct);

            return query; 
        }

        public async Task<IEnumerable<Product>> FilterProducts(  
            string name,
            string? brand,
            int? categoryId,
            int? subCategoryId,
            int? subSubCategoryId,
            decimal? price,
            CancellationToken ct)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(s => s.Category)
                .ThenInclude(s => s.SubCategories)
                .ThenInclude(s => s.SubSubCategories);

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowerName = name.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(lowerName));
            }
             

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(p => p.Brand == brand);

            // Filter by Category
            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(p => p.Category.Id == categoryId.Value);

            // Filter by SubCategory
            if (subCategoryId.HasValue && subCategoryId > 0)
                query = query.Where(p =>
                    p.Category.SubCategories.Any(sc => sc.Id == subCategoryId.Value));

            // Filter by SubSubCategory
            if (subSubCategoryId.HasValue && subSubCategoryId > 0)
                query = query.Where(p =>
                    p.Category.SubCategories
                        .SelectMany(sc => sc.SubSubCategories)
                        .Any(ssc => ssc.Id == subSubCategoryId.Value));

            // Filter by Price
            if (price.HasValue && price > 0)
                query = query.Where(s => s.Price <= price);
            
            return await query.Take(100).ToListAsync(ct);

        }
    }
}
