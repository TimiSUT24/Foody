using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Seeding
{
    public static class IcaDataSeeding
    {
        public static async Task IcaSeed(FoodyDbContext _context)
        {
                
                if(await _context.Products.AnyAsync())
                {
                    Console.WriteLine("Database already seeded");
                    return;
                }
   
                // Path to your JSON file
                var jsonFile = Path.Combine("JsonFiles/Products.json");
                if (!File.Exists(jsonFile))
                {
                    Console.WriteLine("Couldnt seed");
                    return;
                }

                var jsonData = await File.ReadAllTextAsync(jsonFile);
                var productsJson = JsonSerializer.Deserialize<List<IcaProductJson>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (productsJson == null || productsJson.Count == 0) return;

                var existingMainCategory = await _context.Categories
                  .Include(c => c.SubCategories)
                      .ThenInclude(sc => sc.SubSubCategories)
                  .ToListAsync();

            foreach (var prodJson in productsJson)
                {
                
                var mainCategory = existingMainCategory
                    .FirstOrDefault(s => s.MainCategory == prodJson.Categories.MainCategory);

                if (mainCategory == null)
                {
                    mainCategory = new Category
                    {
                        MainCategory = prodJson.Categories.MainCategory,
                        SubCategories = new List<SubCategory>()
                    };
                    existingMainCategory.Add(mainCategory);
                    _context.Categories.Add(mainCategory);
                }

                    SubCategory? subCategory = null;
                    SubSubCategory? subSubCategory = null;

                // --- Handle SubCategory ---
                if (prodJson.Categories.SubCategories?.Any() ?? false)
                {
                    var subCatJson = prodJson.Categories.SubCategories[0];

                    subCategory = mainCategory.SubCategories
                        .FirstOrDefault(s => s.Name == subCatJson.Name);

                    if (subCategory == null)
                    {
                        subCategory = new SubCategory
                        {
                            Name = subCatJson.Name,
                            Category = mainCategory,
                            SubSubCategories = new List<SubSubCategory>()
                        };
                        mainCategory.SubCategories.Add(subCategory);
                    }

                    // --- Handle SubSubCategory ---
                    if (subCatJson.SubSubCategories?.Any() ?? false)
                    {
                        var subSubName = subCatJson.SubSubCategories[0]; // loop if needed

                        // Check DB first
                        subSubCategory = subCategory.SubSubCategories
                            .FirstOrDefault(ssc => ssc.Name == subSubName);

                        if (subSubCategory == null)
                        {
                            subSubCategory = new SubSubCategory
                            {
                                Name = subSubName,
                                SubCategory = subCategory
                            };
                            subCategory.SubSubCategories.Add(subSubCategory);
                        }
                    }
                }
                if (!_context.Products.Any())
                {
                    // --- Create Product ---
                    var product = new Product
                    {
                        Name = prodJson.Name,
                        WeightText = prodJson.WeightText,
                        WeightValue = prodJson.WeightValue,
                        WeightUnit = prodJson.WeightUnit,
                        Ca = prodJson.CaPart,
                        ComparePrice = prodJson.ComparePrice,
                        Price = prodJson.CurrentPrice,
                        ImageUrl = prodJson.ImgUrl,
                        ProductInformation = prodJson.ProductInformation,
                        Country = prodJson.CountryText,
                        Brand = prodJson.CompanyText,
                        Ingredients = prodJson.IngrediensText,
                        Usage = prodJson.Usage,
                        Allergens = prodJson.Allergens,
                        Storage = prodJson.Storage,
                        Stock = 40,
                        IsAvailable = true,
                        Category = mainCategory,
                        SubCategory = subCategory,
                        SubSubCategory = subSubCategory
                    };

                    product.Category = mainCategory;
                    product.SubCategory = subCategory;
                    product.SubSubCategory = subSubCategory;

                    // --- Attributes ---
                    foreach (var attr in prodJson.Attributes ?? new List<string>())
                    {
                        product.ProductAttributes.Add(new ProductAttribute { Value = attr, Food = product });
                    }

                    // --- Nutrition ---
                    foreach (var nut in prodJson.Nutrition ?? new List<NutritionJson>())
                    {
                        string unit = nut.Values?.Keys.FirstOrDefault();   // "100 Gram" or "100 Milliliter"
                        string value = nut.Values?.Values.FirstOrDefault().GetString();
                        product.NutritionValues.Add(new NutritionValue
                        {
                            Name = nut.Näringsvärde,
                            Value = value,
                            NutritionUnitText = prodJson.NutritionUnitText,
                            Food = product

                        });
                    }

                    _context.Products.Add(product);
                }
            }              
                await _context.SaveChangesAsync();         
        }
    }

    public class IcaProductJson
    {
        public string Name { get; set; } = string.Empty;
        public string WeightText { get; set; } = string.Empty;
        public string? CaPart { get; set; }
        public decimal WeightValue { get; set; } 
        public string WeightUnit { get; set; } = string.Empty;
        public string ComparePrice { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public List<string>? Attributes { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string ProductInformation { get; set; } = string.Empty;
        public string CountryText { get; set; } = string.Empty;
        public string CompanyText { get; set; } = string.Empty;
        public string IngrediensText { get; set; } = string.Empty;
        public string Storage { get; set; } = string.Empty;
        public string Usage { get; set; } = string.Empty;
        public string Allergens { get; set; } = string.Empty;
        [JsonPropertyName("nutritionUnitText")]
        public string NutritionUnitText { get; set; } = string.Empty;
        public List<NutritionJson>? Nutrition { get; set; }
        public IcaCategoryJson Categories { get; set; } = new();
    }
    public class NutritionJson
    {
        public string Näringsvärde { get; set; } = string.Empty;
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Values { get; set; } = null!;
    }

    public class IcaCategoryJson
    {
        public string MainCategory { get; set; } = string.Empty;
        public List<IcaSubCategoryJson>? SubCategories { get; set; }
    }

    public class IcaSubCategoryJson
    {
        public string Name { get; set; } = string.Empty;
        public List<string>? SubSubCategories { get; set; }
    }
}
