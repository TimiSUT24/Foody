using Application.Offer.Dto.Request;
using Application.Offer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Offer
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService; 

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer([FromBody] AddOfferDto request, CancellationToken ct)
        {
            var result = await _offerService.AddOffer(request, ct);
            return Ok(result);
        }
    }
}
