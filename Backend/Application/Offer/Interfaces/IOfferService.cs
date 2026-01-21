using Application.Offer.Dto.Request;
using Application.Offer.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Interfaces
{
    public interface IOfferService
    {
        Task<bool> AddOffer(AddOfferDto request, CancellationToken ct);
        Task<bool> DeleteOffer(int id, CancellationToken ct);
        Task<IEnumerable<OfferResponseDto>> GetAllOffers(CancellationToken ct);
        Task<OfferResponseDto> GetOfferById(int id, CancellationToken ct);
    }
}
