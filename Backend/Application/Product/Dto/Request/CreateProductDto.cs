using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Request
{
    public record CreateProductDto(
        string Name,
        string? WeightText,
        decimal? WeightValue,
        string? WeightUnit,
        string? Ca,
        string? ComparePrice,
        decimal Price,
        string? Currency,
        string? ImageUrl,
        string? ProductInformation,
        string? Country,
        string? Brand,
        string? Ingredients,
        string? Storage,
        string? Usage,
        string? Allergens,
        int Stock,
        bool IsAvailable,
        int? CategoryId,
        int? SubCategoryId,
        int? SubSubCategoryId
    );
}
