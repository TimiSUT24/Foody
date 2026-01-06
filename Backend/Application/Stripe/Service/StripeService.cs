using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.StripeChargeShippingOptions.Dto;
using Microsoft.Extensions.Configuration;
using Application.StripeChargeShippingOptions;
using Application.StripeChargeShippingOptions.Interfaces;
using Stripe;

namespace Application.StripeChargeShippingOptions.Service
{
    public class StripeService : IStripeService
    {
        private readonly StripeClient _client;

        public StripeService(IConfiguration config)
        {
            _client = new StripeClient(config["Stripe:SecretKey"]!);
        }

        public async Task<string> CreatePaymentIntentAsync(CreatePaymentRequestDto dto)
        {

            var amountSum = dto.CartItems.Sum(x => x.Price * x.Qty * 100); // convert to cents
            var shippingTaxCents = Math.Round(dto.ShippingTax * 100);
            var subTotal = amountSum + shippingTaxCents;
            var amountWithMoms = subTotal * 1.12m;
            var amount = (long)Math.Round(amountWithMoms);

            var service = new PaymentIntentService(_client);

            var paymentIntent = await service.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "sek",
                PaymentMethodTypes = new List<string> { "card", "klarna" },
                CaptureMethod = "manual",
                Shipping = new ChargeShippingOptions
                {
                    Name = dto.Shipping.FirstName + " " + dto.Shipping.LastName,
                    Phone = dto.Shipping.PhoneNumber,
                    Address = new AddressOptions
                    {
                        Line1 = dto.Shipping.Adress,
                        City = dto.Shipping.City,
                        State = dto.Shipping.State,
                        PostalCode = dto.Shipping.PostalCode,
                        Country = "SE"
                    }
                },
                Metadata = new Dictionary<string, string>
                {       
            { "email", dto.Shipping.Email },
            { "lastname", dto.Shipping.LastName },
            { "deliveryOptionId", dto.Shipping.DeliveryOptionId },
            { "serviceCode", dto.Shipping.ServiceCode },
            { "shippingTax", dto.ShippingTax.ToString() },
            { "userId", dto.UserId }
                }
            });

            return paymentIntent.ClientSecret!;
        }

        public async Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService(_client);
            return await service.CaptureAsync(paymentIntentId);
        }

        public async Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService(_client);
            return await service.CancelAsync(paymentIntentId);
        }
    }

}
