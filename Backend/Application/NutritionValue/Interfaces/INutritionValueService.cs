using Application.NutritionValue.Dto.Request;
using Application.NutritionValue.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Interfaces
{
    public interface INutritionValueService
    {
        Task<bool> AddAsync(CreateNutritionValueDto request, CancellationToken ct);
        Task<IEnumerable<NutritionValueResponse>> GetAllAsync(int foodId, CancellationToken ct);
        Task<NutritionValueResponse> GetByIdAsync(int Id, CancellationToken ct);
        Task<bool> Update(UpdateNutritionValueDto request, CancellationToken ct);
        Task<bool> Delete(int id, CancellationToken ct);
    }
}
