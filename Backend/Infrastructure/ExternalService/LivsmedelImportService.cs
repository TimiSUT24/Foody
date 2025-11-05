using Application.Livsmedel.Dto;
using Application.Livsmedel.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalService
{

    public class LivsmedelImportService : ILivsmedelImportService
    {
        private readonly HttpClient _httpClient;
        private readonly FoodyDbContext _context;
        public LivsmedelImportService(HttpClient httpClient, FoodyDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<LivsmedelListResponse> GetLivsmedelListAsync(int offset, int limit, int sprak)
        {
            var url = $"https://dataportal.livsmedelsverket.se/livsmedel/api/v1/livsmedel?offset={offset}&limit={limit}&sprak={sprak}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var list = await response.Content.ReadFromJsonAsync<LivsmedelListResponse>();
            return list!;
        }

        public async Task<int> ImportLivsmedelBatchAsync(int offset, int limit, int sprak, CancellationToken ct)
        {
            int importedCount = 0;

            //Get list of Livsmedel
            var listResponse = await GetLivsmedelListAsync(offset, limit, sprak);
            if (listResponse?.Livsmedel == null || !listResponse.Livsmedel.Any())
                return importedCount;

            foreach (var foodDto in listResponse.Livsmedel)
            {
                //Check if product already exists
                if (_context.Products.Any(p => p.Number == foodDto.Nummer))
                    continue;

                //Create base product
                var product = new Product
                {
                    Number = foodDto.Nummer,
                    Name = foodDto.Namn,                 
                };

                await FillProductDetailsAsync(product, foodDto.Nummer, sprak);

                //Add to DbContext
                _context.Products.Add(product);
                importedCount++;
            }

            //Save changes to DB
            await _context.SaveChangesAsync(ct);
            return importedCount;
        }

        private async Task FillProductDetailsAsync(Product product, int nummer, int sprak)
        {
            // Nutrition
            var url =
                $"https://dataportal.livsmedelsverket.se/livsmedel/api/v1/livsmedel/{nummer}/naringsvarden?sprak={sprak}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var nutrition = await response.Content.ReadFromJsonAsync<List<NaringsvardeDto>>();

            if (nutrition != null)
            {
                foreach (var n in nutrition)
                {
                    product.NutritionValues.Add(new NutritionValue
                    {
                        Name = n.Namn,
                        EuroFIRCode = n.EuroFIRkod
                    });
                }
            }

            // Ingredients
             var classification = await _httpClient.GetFromJsonAsync<List<KlassificeringDto>>(
                 $"https://dataportal.livsmedelsverket.se/livsmedel/api/v1/livsmedel/{nummer}/klassificeringar?sprak={sprak}");

             if (classification != null)
             {
                 foreach (var i in classification)
                 {
                     product.Classifications.Add(new Classification
                     {
                         Type = i.Typ,
                         Name = i.Namn
                     });
                 }
             }

             // Raw Materials
             var rawMaterials = await _httpClient.GetFromJsonAsync<List<RavaraDto>>(
                 $"https://dataportal.livsmedelsverket.se/livsmedel/api/v1/livsmedel/{nummer}/ravaror?sprak={sprak}");

             if (rawMaterials != null)
             {
                 foreach (var r in rawMaterials)
                 {
                     product.RawMaterials.Add(new RawMaterial
                     {
                         Name = r.Namn,
                         Cooking = r.Tillagning
                     });
                 }
             }

            // Classifications
            var ingredients = await _httpClient.GetFromJsonAsync<List<IngrediensDto>>(
                $"https://dataportal.livsmedelsverket.se/livsmedel/api/v1/livsmedel/{nummer}/ingredienser?sprak={sprak}");

            if (ingredients != null)
            {
                foreach (var c in ingredients)
                {
                    product.Ingredients.Add(new Ingredient
                    {
                        Name = c.Namn,
                        WaterFactor = c.VattenFaktor
                    });
                }
            }
        }
    }
}
