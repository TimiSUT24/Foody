using Application.Ingredient.Dto.Request;
using Application.Ingredient.Dto.Response;
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

        public async Task<bool> AddAsync(CreateIngredientDto request, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(request.FoodId, ct);
            if(existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {request.FoodId} not found.");
            }
            var ingredient = _mapper.Map<Domain.Models.Ingredient>(request);
            ingredient.FoodId = request.FoodId;
            await _uow.Ingredients.AddAsync(ingredient, ct);
            await  _uow.SaveChangesAsync(ct);

            return true; 

        }

        public async Task<IngredientResponse> GetAllAsync(int foodId, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(foodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {foodId} not found.");
            }
            var ingredients = await _uow.Ingredients.GetAllAsync(ct);
            var ingredient = ingredients.FirstOrDefault(i => i.FoodId == foodId);
            if(ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient for food id {foodId} not found.");
            }
            return _mapper.Map<IngredientResponse>(ingredient);
        }

        public async Task<IngredientResponse> GetByIdAsync(int foodId, CancellationToken ct)
        {
            var ingredient = await _uow.Ingredients.GetByIdAsync<int>(foodId, ct);
            if(ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with id {foodId} not found.");
            }
            return _mapper.Map<IngredientResponse>(ingredient);
        }

        public async Task<bool> Update(UpdateIngredientDto request, CancellationToken ct)
        {
            var ingredient = await _uow.Ingredients.GetByIdAsync<int>(request.FoodId, ct);
            if(ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with id {request.FoodId} not found.");
            }
            _mapper.Map(request, ingredient);
            _uow.Ingredients.Update(ingredient);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int foodId, CancellationToken ct)
        {
            var ingredient = await _uow.Ingredients.GetByIdAsync<int>(foodId, ct);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with id {foodId} not found.");
            }
            _uow.Ingredients.Delete(ingredient);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

    }
}
