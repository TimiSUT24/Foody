using Application.Abstractions;
using Application.Offer.Dto.Request;
using Application.Offer.Dto.Response;
using Application.Offer.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Service
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public OfferService(IUnitOfWork uow, ICacheService cache, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<bool> AddOffer(AddOfferDto request, CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync(request.ProductId, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            var mapper = _mapper.Map<Domain.Models.Offer>(request);
            await _uow.Offer.AddAsync(mapper, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("products:");
            await _cache.RemoveByPrefixAsync("category:");

            return true;
        }

        public async Task<IEnumerable<OfferResponseDto>> GetAllOffers(CancellationToken ct)
        {
            var offers = await _uow.Offer.GetAllAsync(ct);
            if(offers == null)
            {
                throw new KeyNotFoundException("No offers was found");
            }

            var mapping = _mapper.Map<IEnumerable<OfferResponseDto>>(offers)
            ?? throw new InvalidOperationException("Mapping failed");

            return mapping;
        }

        public async Task<OfferResponseDto> GetOfferById(int id, CancellationToken ct)
        {
            var offer = await _uow.Offer.GetByIdAsync(id, ct);
            if (offer == null)
            {
                throw new KeyNotFoundException("Offer was not found");
            }

            var mapping = _mapper.Map<OfferResponseDto>(offer) 
                ?? throw new InvalidOperationException("Mapping failed");

            return mapping;
        }

        public async Task<bool> DeleteOffer(int id, CancellationToken ct)
        {
            var offer = await _uow.Offer.GetByIdAsync(id, ct);
            if (offer == null)
            {
                throw new KeyNotFoundException("Offer id was not found");
            }

            _uow.Offer.Delete(offer);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("products:");
            await _cache.RemoveByPrefixAsync("category:");
            return true;
        }
        
    }
}
