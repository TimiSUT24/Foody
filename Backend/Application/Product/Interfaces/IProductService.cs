using Application.Product.Dto.Request;
using Application.Product.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddAsync(CreateProductDto request, CancellationToken ct);
        Task<IEnumerable<ProductResponseDto>> GetAsync(int offset, int limit, CancellationToken ct);
        Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
        Task<bool> Update(UpdateProductDto request, CancellationToken ct);
        Task<ProductDetailsResponse> GetProductDetailsById(int id, CancellationToken ct);
        Task<IEnumerable<string?>> GetBrands(int? categoryId);
        Task<InfiniteScrollResponse<ProductResponseDto>> FilterProducts(string? name, string? brand, int? categoryId, int? subCategoryId, int? subSubCategoryId, decimal? price,bool? offer, int page, int pageSize, CancellationToken ct);
    }
}
