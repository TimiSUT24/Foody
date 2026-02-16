using Api.Controllers.Postnord.Dto;
using Application.Order.Dto.Request;
using Application.Order.Events;
using Application.Order.Handlers;
using Application.Postnord.Dto;
using Application.Stripe.Dto;
using Infrastructure.Data;
using MassTransit;
using MassTransit.Internals;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using xUnitFoody.Common;
using xUnitFoody.Integration.Helpers;
using xUnitFoody.Integration.Seed;

namespace xUnitFoody.Integration
{
    public class OrderCreationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory _factory;

        public OrderCreationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_FullStripeFlow_Returns200()
        {
                //Arrange db reset
                await _factory.Containers.DbReset.ResetAsync();

                //Arrange login user 
                var (token, userId) = await SeedUser.CreateAdminAndLoginAsync(_httpClient, _factory.ServiceProvider);
                Console.WriteLine("create oreder token " + token + "userid:" + userId);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //Arrange Seed Products
                var products = await SeedProductsAsync.SeedProducts(_factory.ServiceProvider, count: 2);
                //Arrange payment request
                var paymentRequest = new CreatePaymentRequestDto
                {
                    CartItems = products.Select(s => new CartItemDto2
                    {
                        Price = s.Price,
                        Name = s.Name,
                        Qty = 3
                    }).ToList(),
                    Shipping = PostnordTestHelper.ValidStripeFalkenberg,
                    ShippingTax = 39m,
                    UserId = userId
                };

                //Act call stripe create payment 
                var paymentResponse = await _httpClient.PostAsJsonAsync("/api/Stripe/create-payment-intent", paymentRequest);
                //Assert
                Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);

                var paymentJson = await paymentResponse.Content.ReadFromJsonAsync<JsonElement>();
                var clientSecret = paymentJson.GetProperty("clientSecret").GetString();
                //Assert
                Assert.False(string.IsNullOrWhiteSpace(clientSecret));

                var retrieveRequest = new RetrievePaymentIntentRequest
                {
                    ClientSecret = clientSecret
                };

                //Act call stripe retrieve payment 
                var retrieveResponse = await _httpClient.PostAsJsonAsync($"/api/Order/retrieve-payment-intent", retrieveRequest);
                //Assert
                Assert.Equal(HttpStatusCode.OK, retrieveResponse.StatusCode);

                var paymentIntentJson = await retrieveResponse.Content.ReadFromJsonAsync<JsonElement>();
                var paymentIntent = paymentIntentJson.GetProperty("id").GetString();
                //Assert
                Assert.False(string.IsNullOrWhiteSpace(paymentIntent));

                //Create Order
                var orderDto = new CreateOrderDto
                {
                    Items = products.Select(s => new CreateOrderItemDto
                    {
                        FoodId = s.Id,
                        Quantity = 2
                    }).ToList(),
                    ShippingInformation = PostnordTestHelper.ValidFalkenberg,
                    PaymentIntentId = paymentIntent

                };

                //call order endpoint
                var orderResponse = await _httpClient.PostAsJsonAsync("/api/Order/create", orderDto);

                //Assert status is 200 ok
                Assert.Equal(HttpStatusCode.OK, orderResponse.StatusCode);
        }

        [Fact]
        public async Task GetPostNordOptions_IncludePostalCodeValidation_ReturnsValidResponse()
        {
            //Arrange db reset
            await _factory.Containers.DbReset.ResetAsync();

            //Arrange PostCode
            var postCodeRequest = new PostalCodeRequest
            {
                PostCode = "31173"
            };

            //Arrange PostCode for deliveryOptions 
            var deliveryPostCode = new DeliveryOptionsRequestDto
            {
                Recipient = new RecipientDto
                {
                    PostCode = "31173"
                }
            };

            //Act call postalCode validation endpoint
            var postCodeResponse = await _httpClient.PostAsJsonAsync("/api/Postnord/postalCode/Validation", postCodeRequest);
            var postCodeResponseJson = await postCodeResponse.Content.ReadFromJsonAsync<ValidationPostalCode>();
            var postCodeValid = postCodeResponseJson!.ValidationResult;
            //Assert ensure status is 200 OK 
            Assert.Equal(HttpStatusCode.OK, postCodeResponse.StatusCode);
            Assert.Equal("VALID", postCodeValid);

            //Act call Postnord delivery options with postalcode 
            var postNordDeliveryResponse = await _httpClient.PostAsJsonAsync("/api/Postnord/options", deliveryPostCode);
            var postNordDeliveryJson = await postNordDeliveryResponse.Content.ReadFromJsonAsync<JsonElement>();
            var postNordDeliveryId = postNordDeliveryJson[0];

            var warehouseId = postNordDeliveryId
                .GetProperty("warehouse")
                .GetProperty("id")
                .GetString();
                
            //Assert ensure status is 200 OK and response is valid
            Assert.Equal(HttpStatusCode.OK, postNordDeliveryResponse.StatusCode);
            Assert.Equal("Falkenberg",warehouseId);
            
        }

    }
}
