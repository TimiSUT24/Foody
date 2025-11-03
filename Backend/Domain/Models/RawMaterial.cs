using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public int FoodId { get; set; }

        public string Name { get; set; } = null!;
        public string? FoodEx2 { get; set; }
        public string? Cooking { get; set; }
        public decimal? Portion { get; set; }
        public decimal? Factor { get; set; }
        public decimal? ConvertedToRaw { get; set; }

        public Product? Food { get; set; }
    }

}
