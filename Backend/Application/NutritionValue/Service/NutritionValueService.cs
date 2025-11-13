using Application.Exceptions;
using Application.NutritionValue.Dto.Request;
using Application.NutritionValue.Dto.Response;
using Application.NutritionValue.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Service
{
    public class NutritionValueService : INutritionValueService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper; 

        public NutritionValueService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateNutritionValueDto request, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(request.FoodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {request.FoodId} not found.");
            }

            var rawmat = await _uow.NutritionValues.GetByName(request.Name, ct);
            if (rawmat != null)
            {
                throw new ConflictException($"Name {request.Name} already exists");
            }

            var nutritionValue = _mapper.Map<Domain.Models.NutritionValue>(request);
            await _uow.NutritionValues.AddAsync(nutritionValue, ct);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<NutritionValueResponse>> GetAllAsync(int foodId , CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(foodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {foodId} not found.");
            }
            var nutritions = await _uow.NutritionValues.GetNutritionValueByFoodIdAsync(foodId, ct);
            if (nutritions == null)
            {
                throw new KeyNotFoundException($"Nutrition for food id {foodId} not found.");
            }
            return _mapper.Map<IEnumerable<NutritionValueResponse>>(nutritions);
        }

        public async Task<NutritionValueResponse> GetByIdAsync(int Id, CancellationToken ct)
        {
            var nutrition = await _uow.NutritionValues.GetByIdAsync<int>(Id, ct);
            if (nutrition == null)
            {
                throw new KeyNotFoundException($"NutritionValue for id {Id} not found.");
            }
            return _mapper.Map<NutritionValueResponse>(nutrition);

        }

        public async Task<bool> Update(UpdateNutritionValueDto request, CancellationToken ct)
        {
            var existNutrition = await _uow.NutritionValues.GetByIdAsync<int>(request.Id, ct);
            if (existNutrition == null)
            {
                throw new KeyNotFoundException($"Nutrition with id {request.Id} not found.");
            }

            _mapper.Map(request, existNutrition);
            _uow.NutritionValues.Update(existNutrition);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken ct)
        {
            var Nutrition = await _uow.NutritionValues.GetByIdAsync(id, ct);
            if (Nutrition == null)
            {
                throw new KeyNotFoundException($"Nutrition with id {id} not found");
            }
            _uow.NutritionValues.Delete(Nutrition);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

    }
}
