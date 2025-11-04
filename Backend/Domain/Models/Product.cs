using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int FoodTypeId { get; set; }
        public string? FoodType { get; set; }
        public int Version { get; set; }
        public string Name { get; set; } = null!;
        public string? ScientificName { get; set; }
        public string? Project { get; set; }
        public string? Analysis { get; set; }
        public string? CookingMethod { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // E-commerce fields
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? DiscountInfo { get; set; }
        public DateTime? OfferValidUntil { get; set; } = DateTime.UtcNow; 
        public string? Supplier { get; set; }
        public string? Origin { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<NutritionValue> NutritionValues { get; set; } = new List<NutritionValue>();
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
        public ICollection<Classification> Classifications { get; set; } = new List<Classification>();

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
