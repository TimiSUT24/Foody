using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeding
{
    using Application.Livsmedel.Dto.Response;
    using Azure.Storage.Blobs;
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Net.Http.Json;

    public class ImageSeeder
    {
        private readonly FoodyDbContext _context;
        private readonly BlobContainerClient _container;
        private readonly HttpClient _http;
        private readonly string _bingApiKey;

        public ImageSeeder(FoodyDbContext context, IConfiguration config)
        {
            _context = context;
            var blobService = new BlobServiceClient(config["AzureStorage:ConnectionString"]);
            _container = blobService.GetBlobContainerClient(config["AzureStorage:ContainerName"]);
            _container.CreateIfNotExists();

            _http = new HttpClient();
            _bingApiKey = config["Bing:ApiKey"]!;
        }

        public async Task RunAsync()
        {
            var products = await _context.Products
                .Where(p => p.ImageUrl == null)
                .ToListAsync();

            Console.WriteLine($"🔍 Hittade {products.Count} produkter utan bilder...");

            foreach (var product in products)
            {
                try
                {
                    string? imageUrl = await GetImageUrlAsync(product.Name);
                    if (imageUrl == null)
                    {
                        Console.WriteLine($"❌ Ingen bild hittad för {product.Name}");
                        continue;
                    }

                    byte[] imageBytes = await _http.GetByteArrayAsync(imageUrl);

                    string blobName = $"{product.Id}-{Guid.NewGuid()}.jpg";
                    var blob = _container.GetBlobClient(blobName);
                    using var ms = new MemoryStream(imageBytes);
                    await blob.UploadAsync(ms, overwrite: true);

                    product.ImageUrl = blob.Uri.ToString();
                    Console.WriteLine($"✅ {product.Name} → {product.ImageUrl}");

                    await _context.SaveChangesAsync();
                    await Task.Delay(200); // undvik throttling
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Fel vid {product.Name}: {ex.Message}");
                }
            }

            Console.WriteLine("🎉 Klart! Alla bilder har laddats upp.");
        }

        private async Task<string?> GetImageUrlAsync(string productName)
        {
            // Försök med Open Food Facts
            string offUrl = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(productName)}&lang=sv&json=1";
            var offResponse = await _http.GetFromJsonAsync<OpenFoodFactsResponse>(offUrl);
            string? imageUrl = offResponse?.Products?.FirstOrDefault(p => !string.IsNullOrEmpty(p.ImageUrl))?.ImageUrl;

            if (!string.IsNullOrEmpty(imageUrl))
                return imageUrl;

            return imageUrl;
        }
    }

}
