using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string? Value { get; set; } = string.Empty;
        public int FoodId { get; set; }
        public Product? Food { get; set; }
    }
}
