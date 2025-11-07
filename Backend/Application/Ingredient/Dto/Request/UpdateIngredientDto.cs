using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Dto.Request
{
    public record UpdateIngredientDto
    (
        int Id,
        string Name,
        decimal? WaterFactor,
        decimal? FatFactor,
        decimal? WeightBeforeCooking,
        decimal? WeightAfterCooking,
        decimal? CookingFactor
    );
}
