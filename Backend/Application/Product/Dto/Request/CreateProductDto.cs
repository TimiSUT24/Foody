using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Request
{
    public record CreateProductDto(
        int Id,
        int FoodTypeId,
        string? FoodType,
        int Version,
        string Name,
        string? ScientificName,
        string? Project,
        string? Analysis,
        string? CookingMethod,
        decimal Price,
        int Stock,
        string? DiscountInfo,
        DateTime? OfferValidUntil,
        string? Supplier,
        string? Origin,
        string? ImageUrl
    );
}
