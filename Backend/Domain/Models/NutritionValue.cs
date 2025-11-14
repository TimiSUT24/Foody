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
        public string Name { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;
        public string? NutritionUnitText { get; set; } = string.Empty; 
        public Product? Food { get; set; }
    }

}
