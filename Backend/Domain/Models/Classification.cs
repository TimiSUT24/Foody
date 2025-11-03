using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Classification
    {
        public int Id { get; set; }
        public int FoodId { get; set; }

        public string? Type { get; set; }
        public string? Facet { get; set; }
        public string? FacetCode { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? LangualId { get; set; }

        public Product? Food { get; set; }
    }

}
