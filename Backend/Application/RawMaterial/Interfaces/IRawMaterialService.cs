using Application.RawMaterial.Dto.Request;
using Application.RawMaterial.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Interfaces
{
    public interface IRawMaterialService
    {
        Task<bool> AddAsync(CreateRawMaterialDto request, CancellationToken ct);
        Task<IEnumerable<RawMaterialResponse>> GetAllAsync(int foodId, CancellationToken ct);
        Task<RawMaterialResponse> GetByIdAsync(int Id, CancellationToken ct);
    }
}
