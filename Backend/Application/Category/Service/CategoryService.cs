using Application.Category.Dto.Response;
using Application.Category.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
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

        public CategoryService (IUnitOfWork unitofwork, IMapper mapper)
        {
            _uow = unitofwork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryTreeResponse>> GetCategoryTree(CancellationToken ct)
        {
            var categoryTree = await _uow.Category.GetCategoryTree(ct);
            if(categoryTree == null)
            {
                throw new KeyNotFoundException("No Categories found");

            }

            var mapping = _mapper.Map<IEnumerable<CategoryTreeResponse>>(categoryTree);

            return mapping; 
        }

        public async Task<CategoryResponse> GetCategoryById(int id, CancellationToken ct)
        {
            var category = await _uow.Category.GetByIdAsync(id,ct);
            if(category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            var mapping = _mapper.Map<CategoryResponse>(category);
            return mapping;

        }

        public async Task<SubCategoryResponse> GetSubCategoryById(int id)
        {
            var subCategory = await _uow.Category.GetSubCategory(id);
            if(subCategory == null)
            {
                throw new KeyNotFoundException("SubCategory not found");
            }

            var mapping = _mapper.Map<SubCategoryResponse>(subCategory);
            return mapping;
        }

        public async Task<SubSubCategoryResponse> GetSubSubCategoryById(int id)
        {
            var subSubCategory = await _uow.Category.GetSubSubCategory(id);
            if(subSubCategory == null)
            {
                throw new KeyNotFoundException("SubSubCategory not found");
            }

            var mapping = _mapper.Map<SubSubCategoryResponse>(subSubCategory);
            return mapping;
        }
        
    }
}
