using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        IProductRepository Products { get; }
        INutritionValueRepository NutritionValues { get; } 
        IOrderRepository Orders { get; }
    }
}
