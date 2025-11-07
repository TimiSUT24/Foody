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
        public IIngredientRepository Ingredients { get; }
        public IRawMaterialRepository RawMaterials { get; }
        public INutritionValueRepository NutritionValues { get; }
        public IClassificationRepository Classifications { get; }

        public UnitOfWork(FoodyDbContext context, 
                        IProductRepository products,
                        IIngredientRepository ingredients,
                        IRawMaterialRepository rawMaterials,
                        INutritionValueRepository nutritionValues,
                        IClassificationRepository classifications)
        {
            _context = context;
            Products = products;
            Ingredients = ingredients;
            RawMaterials = rawMaterials;
            NutritionValues = nutritionValues;
            Classifications = classifications;
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
