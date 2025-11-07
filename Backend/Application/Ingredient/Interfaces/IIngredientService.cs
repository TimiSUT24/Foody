using Application.Ingredient.Dto.Request;
using Application.Ingredient.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Interfaces
{
    public interface IIngredientService
    {
        Task<bool> AddAsync(CreateIngredientDto request, CancellationToken ct);
        Task<IEnumerable<IngredientResponse>> GetAllAsync(int foodId, CancellationToken ct);
        Task<IngredientResponse> GetByIdAsync(int Id, CancellationToken ct);
        Task<bool> Update(UpdateIngredientDto request, CancellationToken ct);
        Task<bool> DeleteAsync(int Id, CancellationToken ct);
    }
}
