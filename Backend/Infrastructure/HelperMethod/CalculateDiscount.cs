using Domain.Enum;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HelperMethod
{
    public class CalculateDiscount : ICalculateDiscount
    {
        public decimal GetFinalPrice(Product product, DateTime utcNow)
        {
            var offer = product.Offer;

            if(offer == null)
            {
                return product.Price;
            }

            if (!offer.IsActive(utcNow))
            {
                return product.Price;
            }
              

            return offer.DiscountType switch
            {
                DiscountType.Percentage =>
                    Math.Round(product.Price * (1 - offer.DiscountValue / 100m), 2),

                DiscountType.FixedAmount =>
                    Math.Max(product.Price - offer.DiscountValue, 0),

                _ => product.Price
            };
        }
    }
}
