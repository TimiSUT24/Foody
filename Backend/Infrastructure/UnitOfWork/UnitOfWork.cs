using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly FoodyDbContext _context;

        public IProductRepository Products { get; }
        public INutritionValueRepository NutritionValues { get; }
        public IOrderRepository Orders { get; }
        public ICategoryRepository Category { get; }
        public IUserRepository User { get; }

        public UnitOfWork(FoodyDbContext context, 
                        IProductRepository products,
                        INutritionValueRepository nutritionValues,
                        IOrderRepository orders,
                        ICategoryRepository category,
                        IUserRepository user)
        {
            _context = context;
            Products = products;
            NutritionValues = nutritionValues;
            Orders = orders;
            Category = category;
            User = user;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
