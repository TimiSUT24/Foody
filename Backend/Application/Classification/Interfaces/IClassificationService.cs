using Application.Classification.Dto.Request;
using Application.Classification.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Interfaces
{
    public interface IClassificationService
    {
        Task<bool> AddAsync(CreateClassificationDto request, CancellationToken ct);
        Task<IEnumerable<ClassificationResponse>> GetAllAsync(int foodId, CancellationToken ct);
        Task<ClassificationResponse> GetByIdAsync(int Id, CancellationToken ct);
        Task<bool> Update(UpdateClassificationDto request, CancellationToken ct);
        Task<bool> Delete(int Id, CancellationToken ct);
    }
}
