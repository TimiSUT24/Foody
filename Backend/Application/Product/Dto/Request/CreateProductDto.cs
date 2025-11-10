using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Dto.Request
{
    public class CreateProductDto
    {
        public int FoodTypeId { get; set; }
        public string? FoodType { get; set; }
        public int Version { get; set; }
        public string Name { get; set; } = null!;
        public string? ScientificName { get; set; }
        public string? Project { get; set; }
        public string? Analysis { get; set; }
        public string? CookingMethod { get; set; }

        // E-commerce fields
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? DiscountInfo { get; set; }
        public DateTime? OfferValidUntil { get; set; }
        public string? Supplier { get; set; }
        public string? Origin { get; set; }
        public string? ImageUrl { get; set; }
    }
}
