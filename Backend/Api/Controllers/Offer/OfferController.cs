using Application.Offer.Dto.Request;
using Application.Offer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Offer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService; 

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:400)]
        [HttpPost]
        public async Task<IActionResult> AddOffer([FromBody] AddOfferDto request, CancellationToken ct)
        {
            var result = await _offerService.AddOffer(request, ct);
            return Ok(result);
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [HttpGet]
        public async Task<IActionResult> GetAllOffers(CancellationToken ct)
        {
            var result = await _offerService.GetAllOffers(ct);
            return Ok(result);
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOfferById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _offerService.GetOfferById(id,ct);
            return Ok(result);
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteOffer([FromRoute] int id, CancellationToken ct)
        {
            var result = await _offerService.DeleteOffer(id, ct);
            return Ok(result);
        }
    }
}
