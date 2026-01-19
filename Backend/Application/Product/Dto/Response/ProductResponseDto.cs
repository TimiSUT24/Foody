using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Response
{
    public record ProductResponseDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string WeightText { get; init; } = string.Empty;
        public decimal WeightValue { get; init; }
        public string WeightUnit { get; init; } = string.Empty;
        public string Ca { get; init; } = string.Empty;
        public string ComparePrice { get; init; } = string.Empty;
        public string Currency { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
        public string ProductInformation { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Ingredients { get; init; } = string.Empty;
        public string Storage { get; init; } = string.Empty;
        public string Usage { get; init; } = string.Empty;
        public string Allergens { get; init; } = string.Empty;
        public bool IsAvailable { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public int CategoryId { get; init; }
        public int SubCategoryId { get; init; }
        public int SubSubCategoryId { get; init; }  
        public decimal FinalPrice { get; init; } 
        public bool HasOffer { get; init; }
    }
}
