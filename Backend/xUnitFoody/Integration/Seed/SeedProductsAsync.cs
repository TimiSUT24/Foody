using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitFoody.Integration.Seed
{
    public static class SeedProductsAsync
    {
        public static async Task<List<Product>> SeedProducts(IServiceProvider serviceProvider, int count)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FoodyDbContext>();

            //Add category
            var category = new Category { Id = 1, MainCategory = "Test Category" };
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            //Add products
            var products = Enumerable.Range(1, count).Select(i => new Product
            {
                Name = $"Product {i}",
                Price = 10m * i,
                ProductInformation = $"Description {i}",
                CategoryId = category.Id,
                Stock = 100,
            }).ToList();

            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();

            return products;
        }
    }
}
