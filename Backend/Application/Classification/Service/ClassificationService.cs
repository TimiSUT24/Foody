using Application.Classification.Dto.Request;
using Application.Classification.Dto.Response;
using Application.Classification.Interfaces;
using Application.RawMaterial.Dto.Request;
using Application.RawMaterial.Dto.Response;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Service
{
    public class ClassificationService : IClassificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ClassificationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateClassificationDto request, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(request.FoodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {request.FoodId} not found.");
            }
            var classification = _mapper.Map<Domain.Models.Classification>(request);
            await _uow.Classifications.AddAsync(classification, ct);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<ClassificationResponse>> GetAllAsync(int foodId , CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(foodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {foodId} not found.");
            }
            var classification = await _uow.Classifications.GetClassificationsByFoodIdAsync(foodId, ct);
            if (classification == null)
            {
                throw new KeyNotFoundException($"Classification for food id {foodId} not found.");
            }
            return _mapper.Map<IEnumerable<ClassificationResponse>>(classification);
        }

        public async Task<ClassificationResponse> GetByIdAsync(int Id, CancellationToken ct)
        {
            var classification = await _uow.Classifications.GetByIdAsync<int>(Id, ct);
            if (classification == null)
            {
                throw new KeyNotFoundException($"Classification id {Id} not found.");
            }
            return _mapper.Map<ClassificationResponse>(classification);

        }

        public async Task<bool> Update(UpdateClassificationDto request, CancellationToken ct)
        {
            var classification = await _uow.Classifications.GetByIdAsync<int>(request.Id, ct);
            if (classification == null)
            {
                throw new KeyNotFoundException($"Classification with id {request.Id} not found.");
            }

            _mapper.Map(request, classification);
            _uow.Classifications.Update(classification);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> Delete(int Id, CancellationToken ct)
        {
            var classification = await _uow.Classifications.GetByIdAsync(Id, ct);
            if (classification == null)
            {
                throw new KeyNotFoundException($"Classification with id {Id} not found");
            }
            _uow.Classifications.Delete(classification);
            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
