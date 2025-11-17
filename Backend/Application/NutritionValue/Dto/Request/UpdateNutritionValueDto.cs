using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Dto.Request
{
    public record UpdateNutritionValueDto(
       int Id,
       string Name,
       string? Value,
       string? NutritionUnitText
       );
}
