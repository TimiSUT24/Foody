using Application.Offer.Dto.Request;
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

        public OfferService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddOffer(AddOfferDto request, CancellationToken ct)
        {
            var product = _uow.Products.GetByIdAsync(request.ProductId, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            var mapper = _mapper.Map<Domain.Models.Offer>(request);
            await _uow.Offer.AddAsync(mapper, ct);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
        
    }
}
