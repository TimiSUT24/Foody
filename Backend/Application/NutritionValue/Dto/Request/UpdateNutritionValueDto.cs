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
       string? EuroFIRCode,
       string? Abbreviation,
       decimal? Value,
       string? Unit,
       decimal? WeightGram,
       string? MatrixUnit,
       string? MatrixUnitCode,
       string? Calculation,
       string? ValueType,
       string? ValueTypeCode,
       string? Origin,
       string? OriginCode,
       string? Publication,
       string? MethodType,
       string? MethodTypeCode,
       string? MethodIndicator,
       string? MethodIndicatorCode,
       string? ReferenceType,
       string? ReferenceTypeCode,
       string? Comment
       );
}
