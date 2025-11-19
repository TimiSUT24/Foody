using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoryTree(CancellationToken ct);
        Task<SubCategory> GetSubCategory(int id);
        Task<SubSubCategory> GetSubSubCategory(int id);
    }
}
