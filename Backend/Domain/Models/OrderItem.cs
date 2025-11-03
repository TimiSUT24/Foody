using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int FoodId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order? Order { get; set; }
        public Product? Food { get; set; }
    }

}
