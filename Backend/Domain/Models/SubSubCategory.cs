using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SubSubCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public ICollection<Product> Food {  get; set; } = new List<Product>();
    }
}
