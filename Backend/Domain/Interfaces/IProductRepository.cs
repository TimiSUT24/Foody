using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetProductDetailsById(int id, CancellationToken ct);
        Task<IEnumerable<string?>> GetBrands(int? categoryId);
        Task<(List<Product> Items, bool HasMore)> FilterProducts(
           string name,
           string? brand,
           int? categoryId,
           int? subCategoryId,
           int? subSubCategoryId,
           decimal? price,
           int page,
           int pageSize,
           CancellationToken ct);
    }
}
