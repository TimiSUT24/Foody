using Application.Product.Dto.Request;
using MassTransit.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using xUnitFoody.Common;
using xUnitFoody.Integration.Seed;

namespace xUnitFoody.Integration
{
    public class CreateProductTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory _factory;

        public CreateProductTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_AsAdmin_Returns201()
        {

            //Arrange reset db 
            await _factory.Containers.DbReset.ResetAsync();

            //Arrange create admin and login as one
            var token = await SeedUser.CreateAdminAndLoginAsync(_httpClient, _factory.ServiceProvider);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new CreateProductDto
            (
                Name: "Milk",
                WeightText: "1L",
                WeightValue: 1,
                WeightUnit: "L",
                Ca: null,
                ComparePrice: null,
                Price: 12.95m,
                Currency: "SEK",
                ImageUrl: null,
                ProductInformation: "Test product",
                Country: "Sweden",
                Brand: "TestBrand",
                Ingredients: "Milk",
                Storage: "Cold",
                Usage: "Drink",
                Allergens: "Milk",
                Stock: 10,
                IsAvailable: true,
                CategoryId: null,
                SubCategoryId: null,
                SubSubCategoryId: null
            );

            //Act 
            var response = await _httpClient.PostAsJsonAsync("/api/product/create", dto);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }
        
    }
}
