using Application.Category.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryTreeResponse>> GetCategoryTree(CancellationToken ct);
        Task<CategoryResponse> GetCategoryById(int id, CancellationToken ct);
        Task<SubCategoryResponse> GetSubCategoryById(int id);
        Task<SubSubCategoryResponse> GetSubSubCategoryById(int id);
    }
}
