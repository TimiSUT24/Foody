using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class NutritionValue
    {
        public int Id { get; set; }
        public int FoodId { get; set; }

        public string Name { get; set; } = null!;
        public string? EuroFIRCode { get; set; }
        public string? Abbreviation { get; set; }
        public decimal? Value { get; set; }
        public string? Unit { get; set; }
        public decimal? WeightGram { get; set; }
        public string? MatrixUnit { get; set; }
        public string? MatrixUnitCode { get; set; }
        public string? Calculation { get; set; }
        public string? ValueType { get; set; }
        public string? ValueTypeCode { get; set; }
        public string? Origin { get; set; }
        public string? OriginCode { get; set; }
        public string? Publication { get; set; }
        public string? MethodType { get; set; }
        public string? MethodTypeCode { get; set; }
        public string? MethodIndicator { get; set; }
        public string? MethodIndicatorCode { get; set; }
        public string? ReferenceType { get; set; }
        public string? ReferenceTypeCode { get; set; }
        public string? Comment { get; set; }

        public Product? Food { get; set; }
    }

}
