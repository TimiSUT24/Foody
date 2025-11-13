using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<SubSubCategory> SubSubCategories { get; set; } = new List<SubSubCategory>();
        public ICollection<Product> Food { get; set; } = new List<Product>();   
    }
}
