using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Response
{
    public record ProductResponseDto()
    {
        public int Id { get; init; }
        public int FoodTypeId { get; init; }
        public string FoodType { get; init; } = string.Empty;
        public string Version { get; init; } = string.Empty;
        public string Name { get; init; } = null!;
        public string ScientificName { get; init; } = string.Empty;
        public string Project { get; init; } = string.Empty;
        public string Analysis { get; init; } = string.Empty;
        public string CookingMethod { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public string DiscountInfo { get; init; } = string.Empty;
        public DateTime? OfferValidUntil { get; init; }
        public string Supplier { get; init; } = string.Empty;
        public string Origin { get; init; } = string.Empty;
        public string ImageUrl { get; init; } = string.Empty;
    }
}
