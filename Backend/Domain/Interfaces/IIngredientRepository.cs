using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IIngredientRepository : IGenericRepository<Ingredient>
    {
        Task<IEnumerable<Ingredient>> GetIngredientsByFoodIdAsync(int foodId, CancellationToken ct);
    }
}
