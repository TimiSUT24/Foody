using Application.Abstractions;
using Application.Exceptions;
using Application.NutritionValue.Dto.Response;
using Application.Product.Dto.Request;
using Application.Product.Dto.Response;
using Application.Product.Interfaces;
using AutoMapper;
using Domain.Enum;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICalculateDiscount _discount;
        private readonly ICacheService _cache;
        private readonly ILogger<ProductService> _logger;
        private readonly CacheSettings _cacheSettings;

        public ProductService(IUnitOfWork uow, IMapper mapper, ICalculateDiscount discount, ICacheService cache, ILogger<ProductService> logger, IOptions<CacheSettings> cacheSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _discount = discount;
            _cache = cache;
            _logger = logger;
            _cacheSettings = cacheSettings.Value;
        }

        public async Task<bool> AddAsync(CreateProductDto request, CancellationToken ct)
        {
            var exists = await _uow.Products.AnyAsync(p => p.Name == request.Name, ct);
            if (exists)
            {
                throw new ConflictException("Product already exists");
            }
            var product = _mapper.Map<Domain.Models.Product>(request);
            await _uow.Products.AddAsync(product, CancellationToken.None);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("products:");
            await _cache.RemoveByPrefixAsync("category:");
            return true;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAsync(int offset, int limit, CancellationToken ct)
        {
            var products = await _uow.Products.GetAsync(offset, limit, ct);
            if(products == null || !products.Any())
            {
                throw new KeyNotFoundException("No products found");
            }
            var mapping = _mapper.Map<IEnumerable<ProductResponseDto>>(products)
                ?? throw new InvalidOperationException("Mapping failed");
            return mapping;
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(id, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            var mapping = _mapper.Map<ProductResponseDto>(product)
                ?? throw new InvalidOperationException("Mapping failed");
            return mapping;
        }

        public async Task<ProductDetailsResponse> GetProductDetailsById(int id, CancellationToken ct)
        {
            var cacheKey = $"details:{id.ToString()}";

            return await _cache.GetOrCreateAsync("products:",cacheKey, async _ =>
            {
                var product = await _uow.Products.GetProductDetailsById(id, ct);
                if (product == null)
                {
                    throw new KeyNotFoundException("Couldnt find ProductDetails");
                }

                var now = DateTime.UtcNow;
                var finalPrice = _discount.GetFinalPrice(product, now);
                var mapping = new ProductDetailsResponse
                {

                    Product = new ProductResponseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        FinalPrice = finalPrice,
                        HasOffer = finalPrice < product.Price,
                        ImageUrl = product.ImageUrl,
                        ComparePrice = product.ComparePrice,
                        Currency = product.Currency,
                        WeightText = product.WeightText,
                        WeightValue = (decimal)product.WeightValue,
                        Ingredients = product.Ingredients,
                        Usage = product.Usage,
                        Storage = product.Storage,
                        Allergens = product.Allergens,
                        Brand = product.Brand,
                        Country = product.Country,
                        ProductInformation = product.ProductInformation,
                        OfferName = product.Offer?.Name,
                        CategoryId = (int)product.CategoryId,
                        SubCategoryId = (int)product.SubCategoryId,
                        SubSubCategoryId = (int)product.SubSubCategoryId

                    },
                    Nutrition = product.NutritionValues?.Select(item => new NutritionValueResponse
                    {
                        Name = item.Name,
                        Value = item.Value,
                        NutritionUnitText = item.NutritionUnitText,
                        Id = item.Id

                    }).ToList(),

                    Attribute = product.ProductAttributes?.Select(item => new ProductAttributeResponse
                    {
                        Id = item.Id,
                        Value = item.Value
                    }).ToList()

                };

                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );                      
        }

        public async Task<InfiniteScrollResponse<ProductResponseDto>> FilterProducts(string? name, string? brand,int? categoryId,int? subCategoryId,int? subSubCategoryId,decimal? price,bool? offer, int page, int pageSize, CancellationToken ct)
        {
            var cacheKey = BuildFilterCacheKey(name, brand, categoryId, subCategoryId,subSubCategoryId, price, offer, page, pageSize);

            return await _cache.GetOrCreateAsync("products:",cacheKey, async _ =>
            {
                _logger.LogInformation("Cache Miss: Fetching from db");
                var (items, hasMore) = await _uow.Products.FilterProducts(name ?? "", brand, categoryId, subCategoryId, subSubCategoryId, price, offer, page, pageSize, ct);

                var now = DateTime.UtcNow;
                var mapping = items.Select(product =>
                {
                    var finalPrice = _discount.GetFinalPrice(product, now);

                    return new ProductResponseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        FinalPrice = finalPrice,
                        HasOffer = finalPrice < product.Price,
                        ImageUrl = product.ImageUrl,
                        ComparePrice = product.ComparePrice,
                        Currency = product.Currency,
                        WeightText = product.WeightText,
                        WeightValue = (decimal)product.WeightValue,
                        IsAvailable = product.Stock > 0,
                        OfferName = product.Offer?.Name
                    };
                }).ToList();

                return new InfiniteScrollResponse<ProductResponseDto>
                {
                    Items = mapping,
                    HasMore = hasMore
                };
            },
                TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
            
        }

        public async Task<bool> Update(UpdateProductDto request, CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(request.Id, ct);
            if(product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            
            _mapper.Map(request, product);
            _uow.Products.Update(product);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("products:");
            await _cache.RemoveByPrefixAsync("category:");
            return true; 
        }

        public async Task<bool> DeleteAsync(int id ,CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(id, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            _uow.Products.Delete(product);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("products:");
            await _cache.RemoveByPrefixAsync("category:");
            return true;
        }

        public async Task<IEnumerable<string?>> GetBrands(int? categoryId)
        {
            var cacheKey = $"brands:{categoryId?.ToString()}";
            return await _cache.GetOrCreateAsync("products:",cacheKey, async _ =>
            {
                var brands = await _uow.Products.GetBrands(categoryId);
                if (brands == null)
                {
                    throw new KeyNotFoundException("No brands found");
                }

                var mapping = _mapper.Map<IEnumerable<string?>>(brands);
                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );

        }

        public async Task<IReadOnlyList<Domain.Models.Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct)
        {
            var cacheKey = $"byids:{string.Join(",", ids.OrderBy(x => x))}";
            return await _cache.GetOrCreateAsync("products:",cacheKey, async _ =>
            {
                var products = await _uow.Products.GetByIdsAsync(ids, ct);
                if (products == null || !products.Any())
                {
                    throw new KeyNotFoundException("No products found");
                }
                return products;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
                     
        }

        private static string BuildFilterCacheKey(string? name, string? brand, int? categoryId, int? subCategoryId, int? subSubCategoryId, decimal? price, bool? offer, int page, int pageSize)
        {
            return $"filter:" +
           $"name={name ?? "null"}|" +
           $"brand={brand ?? "null"}|" +
           $"cat={categoryId?.ToString() ?? "null"}|" +
           $"sub={subCategoryId?.ToString() ?? "null"}|" +
           $"subsub={subSubCategoryId?.ToString() ?? "null"}|" +
           $"price={price?.ToString() ?? "null"}|" +
           $"offer={offer?.ToString() ?? "null"}|" +
           $"page={page}|size={pageSize}";
        }
    }
}
