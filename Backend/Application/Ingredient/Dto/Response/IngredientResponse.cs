using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Dto.Response
{
    public class IngredientResponse
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? WaterFactor { get; set; }
        public decimal? FatFactor { get; set; }
        public decimal? WeightBeforeCooking { get; set; }
        public decimal? WeightAfterCooking { get; set; }
        public string? CookingFactor { get; set; }
    }
}
