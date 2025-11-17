using Application.NutritionValue.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Response
{
    public record ProductDetailsResponse
    {
        public ProductResponseDto? Product { get; init; }
        public List<NutritionValueResponse>? Nutrition { get; init; }

    }
}
