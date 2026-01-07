using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Moms { get; set; }
        public decimal ShippingTax { get; set; } 
        public decimal TotalWeight { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

        public User? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ShippingInformation ShippingInformation { get; set; } = new ShippingInformation();
    }

}
