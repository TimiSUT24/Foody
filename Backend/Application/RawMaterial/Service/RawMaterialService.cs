using Application.RawMaterial.Dto.Request;
using Application.RawMaterial.Dto.Response;
using Application.RawMaterial.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Service
{
    public class RawMaterialService : IRawMaterialService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RawMaterialService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateRawMaterialDto request, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(request.FoodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {request.FoodId} not found.");
            }
            var rawMaterial = _mapper.Map<Domain.Models.RawMaterial>(request);
            await _uow.RawMaterials.AddAsync(rawMaterial, ct);
            await _uow.SaveChangesAsync(ct);
            return true;

        }

        public async Task<IEnumerable<RawMaterialResponse>> GetAllAsync(int foodId, CancellationToken ct)
        {
            var existFood = await _uow.Products.GetByIdAsync<int>(foodId, ct);
            if (existFood == null)
            {
                throw new KeyNotFoundException($"Food with id {foodId} not found.");
            }
            var rawMaterials = await _uow.RawMaterials.GetAllAsync(ct);
            var rawMaterial = rawMaterials.Where(rm => rm.FoodId == foodId);
            if (rawMaterial == null)
            {
                throw new KeyNotFoundException($"Raw material for food id {foodId} not found.");
            }
            return _mapper.Map<IEnumerable<RawMaterialResponse>>(rawMaterial);
        }

        public async Task<RawMaterialResponse> GetByIdAsync(int foodId, CancellationToken ct)
        {
            var rawMaterial = await _uow.RawMaterials.GetByIdAsync<int>(foodId, ct);
            if (rawMaterial == null)
            {
                throw new KeyNotFoundException($"Raw material for food id {foodId} not found.");
            }
            return _mapper.Map<RawMaterialResponse>(rawMaterial);

        }
    }
}
