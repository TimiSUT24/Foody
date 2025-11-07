using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string Name { get; set; } = null!;
        public decimal? WaterFactor { get; set; }
        public decimal? FatFactor { get; set; }
        public decimal? WeightBeforeCooking { get; set; }
        public decimal? WeightAfterCooking { get; set; }
        public string CookingFactor { get; set; } = string.Empty;

        // Retention factors stored as JSON if complex
        public string? RetentionFactorsJson { get; set; }

        public Product? Food { get; set; }
    }

}
