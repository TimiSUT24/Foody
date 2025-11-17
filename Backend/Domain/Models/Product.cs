using Microsoft.Identity.Client;
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
        public string Name { get; set; } = string.Empty;
        public string? WeightText { get; set; } = string.Empty;
        public decimal? WeightValue { get; set; }
        public string? WeightUnit { get; set; } = string.Empty;
        public string? Ca { get; set; } = string.Empty; 
        public string? ComparePrice {  get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Currency { get; set; } = "SEK";
        public string? ImageUrl { get; set; } = string.Empty;
        public string? ProductInformation { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? Brand {  get; set; } = string.Empty;
        public string? Ingredients {  get; set; } = string.Empty;
        public string? Storage {  get; set; } = string.Empty;
        public string? Usage {  get; set; } = string.Empty;
        public string? Allergens {  get; set; } = string.Empty;
        public int Stock {  get; set; }
        public bool IsAvailable { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public int? SubSubCategoryId { get; set; }
        public SubSubCategory? SubSubCategory { get; set; }

        public ICollection<NutritionValue> NutritionValues { get; set; } = new List<NutritionValue>();
        public ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
