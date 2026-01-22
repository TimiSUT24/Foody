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
                .Include(s => s.Offer)
                .Include(s => s.ProductAttributes)
                .Include(s => s.NutritionValues)
                .Include(s => s.Category).ThenInclude(s => s.SubCategories).ThenInclude(s => s.SubSubCategories)
                .FirstOrDefaultAsync(s => s.Id == id, ct);

            return query; 
        }

        public async Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(s => s.Offer)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<string?>> GetBrands(int? categoryId)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId);
            }

            return await query
                .AsNoTracking()
                .Select(s => s.Brand)
                .Where(s => s != null && s != "")
                .Distinct()
                .ToListAsync();
        }
   
        public async Task<(List<Product> Items, bool HasMore)> FilterProducts(  
            string name,
            string? brand,
            int? categoryId,
            int? subCategoryId,
            int? subSubCategoryId,
            decimal? price,
            bool? offer,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            IQueryable<Product> query = _context.Products
                .AsNoTracking()
                .Include(s => s.Offer)
                .Include(s => s.Category)
                .ThenInclude(s => s.SubCategories)
                .ThenInclude(s => s.SubSubCategories);

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowerName = name.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(lowerName));
            }

            query = query.Where(s => s.Stock > 0);

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(p => p.Brand == brand);

            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (subCategoryId.HasValue && subCategoryId > 0)
                query = query.Where(p => p.SubCategoryId == subCategoryId.Value);

            if (subSubCategoryId.HasValue && subSubCategoryId > 0)
                query = query.Where(p => p.SubSubCategoryId == subSubCategoryId.Value);

            // Filter by Price
            if (price.HasValue && price > 0)
                query = query.Where(s => s.Price >= price);

            var utcNow = DateTime.UtcNow;
            if(offer == true)
            {
                query = query.Where(s => s.Offer != null && s.Offer.StartsAtUtc <= utcNow && s.Offer.EndsAtUtc >= utcNow);

            }

                var items = await query
                    .OrderBy(p => p.Price)
                    .ThenBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize + 1)
                    .ToListAsync(ct);

            bool hasMore = items.Count > pageSize;

            return (
                Items: items.Take(pageSize).ToList(),
                HasMore: hasMore
                );

        }
    }
}
