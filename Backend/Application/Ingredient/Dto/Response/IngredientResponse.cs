using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Dto.Response
{
    public record IngredientResponse(
        int Id,
        string Name,
        decimal? WaterFactor,
        decimal? FatFactor,
        decimal? WeightBeforeCooking,
        decimal? WeightAfterCooking,
        string? CookingFactor
        );
}
