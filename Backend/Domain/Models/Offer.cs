using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; } 

        public decimal DiscountValue { get; set; } 
        public DateTime StartsAtUtc { get; set; }
        public DateTime EndsAtUtc { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public bool IsActive(DateTime utcNow) =>
            utcNow >= StartsAtUtc && utcNow <= EndsAtUtc;


    }
}
