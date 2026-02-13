using Application.Abstractions;
using Application.Category.Dto.Response;
using Application.Category.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly CacheSettings _cacheSettings;

        public CategoryService (IUnitOfWork unitofwork, IMapper mapper, ICacheService cache,IOptions<CacheSettings> cacheSettings)
        {
            _uow = unitofwork;
            _mapper = mapper;
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
        }

        public async Task<IEnumerable<CategoryTreeResponse>> GetCategoryTree(CancellationToken ct)
        {
            var cacheKey = $"tree:";
            return await _cache.GetOrCreateAsync("category:",cacheKey, async _ =>
            {
                var categoryTree = await _uow.Category.GetCategoryTree(ct);
                if (categoryTree == null)
                {
                    throw new KeyNotFoundException("No Categories found");

                }

                var mapping = _mapper.Map<IEnumerable<CategoryTreeResponse>>(categoryTree);

                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
            
        }

        public async Task<CategoryResponse> GetCategoryById(int id, CancellationToken ct)
        {
            var cacheKey = $"id:{id.ToString()}";
            return await _cache.GetOrCreateAsync("category:",cacheKey, async _ =>
            {
                var category = await _uow.Category.GetByIdAsync(id, ct);
                if (category == null)
                {
                    throw new KeyNotFoundException("Category not found");
                }

                var mapping = _mapper.Map<CategoryResponse>(category);
                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );          
        }

        public async Task<SubCategoryResponse> GetSubCategoryById(int id)
        {
            var cacheKey = $"subCategory:id:{id.ToString()}";
            return await _cache.GetOrCreateAsync("category:", cacheKey, async _ =>
            {
                var subCategory = await _uow.Category.GetSubCategory(id);
                if (subCategory == null)
                {
                    throw new KeyNotFoundException("SubCategory not found");
                }

                var mapping = _mapper.Map<SubCategoryResponse>(subCategory);
                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
            
        }

        public async Task<SubSubCategoryResponse> GetSubSubCategoryById(int id)
        {
            var cacheKey = $"subsubCategory:id:{id.ToString()}";
            return await _cache.GetOrCreateAsync("category:",cacheKey, async _ =>
            {
                var subSubCategory = await _uow.Category.GetSubSubCategory(id);
                if (subSubCategory == null)
                {
                    throw new KeyNotFoundException("SubSubCategory not found");
                }

                var mapping = _mapper.Map<SubSubCategoryResponse>(subSubCategory);
                return mapping;
            },
            TimeSpan.FromMinutes(_cacheSettings.LongLivedMinutes)
            );
            
        }      
    }
}
