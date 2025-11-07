using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Dto.Response
{
    public record NutritionValueResponse
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? EuroFIRCode { get; init; }
        public string? Abbreviation { get; init; }
        public decimal? Value { get; init; }
        public string? Unit { get; init; }
        public decimal? WeightGram { get; init; }
        public string? MatrixUnit { get; init; }
        public string? MatrixUnitCode { get; init; }
        public string? Calculation { get; init; }
        public string? ValueType { get; init; }
        public string? ValueTypeCode { get; init; }
        public string? Origin { get; init; }
        public string? OriginCode { get; init; }
        public string? Publication { get; init; }
        public string? MethodType { get; init; }
        public string? MethodTypeCode { get; init; }
        public string? MethodIndicator { get; init; }
        public string? MethodIndicatorCode { get; init; }
        public string? ReferenceType { get; init; }
        public string? ReferenceTypeCode { get; init; }
        public string? Comment { get; init; }
    }
}
