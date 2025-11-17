using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Dto.Request
{
    public record CreateNutritionValueDto(
        int FoodId,
        string Name,
        string? Value,
        string? NutritionUnitText
        );
}
