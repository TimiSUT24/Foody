using Application.Offer.Dto.Request;
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
    }
}
