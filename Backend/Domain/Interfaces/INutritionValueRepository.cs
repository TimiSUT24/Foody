using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INutritionValueRepository : IGenericRepository<NutritionValue>
    {
        Task<IEnumerable<NutritionValue>> GetNutritionValueByFoodIdAsync(int foodId, CancellationToken ct);
        Task<NutritionValue> GetByName(string name, CancellationToken ct);
    }
}
