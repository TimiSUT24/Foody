using Api.Controllers.Postnord.Dto;
using Application.Postnord.Dto;
using Application.Postnord.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Postnord
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostnordController : ControllerBase
    {
        private readonly IPostnordService _postNord;

        public PostnordController(IPostnordService postNord)
        {
            _postNord = postNord;
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [HttpPost("options")]
        public async Task<IActionResult> GetOptions([FromBody] DeliveryOptionsRequestDto dto)
        {
            if (dto.Recipient == null)
                return BadRequest("recipient is required");

            var result = await _postNord.GetDeliveryOptionsAsync(dto.Recipient.PostCode);
            return Ok(result);
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [HttpPost("booking")]
        public async Task<IActionResult> Book([FromBody] PostNordBookingRequestDto dto,CancellationToken ct)
        {
            if (dto.Shipping == null)
                return BadRequest("Shipping info required");

            var result = await _postNord.BookShipmentAsync(dto,ct);
            return Ok(result);
        }

        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:400)]
        [HttpPost("postalCode/Validation")]
        public async Task<IActionResult> ValidatePostalCode(PostalCodeRequest dtoRequest)
        {
            var result = await _postNord.ValidatePostalCode(dtoRequest);
            return Ok(result);
        }
    }
}
