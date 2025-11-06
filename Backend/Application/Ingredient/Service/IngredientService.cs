using Application.Ingredient.Dto.Request;
using Application.Ingredient.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Service
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public IngredientService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(int foodId, CreateIngredientDto request, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(foodId, ct);
            if(existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {foodId} not found.");
            }
            var ingredient = _mapper.Map<Domain.Models.Ingredient>(request);
            ingredient.FoodId = foodId;
            await _uow.Ingredients.AddAsync(ingredient, ct);
            await  _uow.SaveChangesAsync(ct);

            return true; 

        }

    }
}
