using Application.StripeChargeShippingOptions.Dto;
using Application.StripeChargeShippingOptions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Stripe
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentRequestDto dto)
        {            
                var clientSecret = await _stripeService.CreatePaymentIntentAsync(dto);
                return Ok(new { clientSecret });                   
        }

        [HttpPost("capture-payment-intent")]
        public async Task<IActionResult> CapturePaymentIntent([FromBody] PaymentIntentIdDto dto)
        {       
                var intent = await _stripeService.CapturePaymentIntentAsync(dto.PaymentIntentId);
                return Ok(intent);         
        }

        [HttpPost("cancel-payment-intent")]
        public async Task<IActionResult> CancelPaymentIntent([FromBody] PaymentIntentIdDto dto)
        {         
                var intent = await _stripeService.CancelPaymentIntentAsync(dto.PaymentIntentId);
                return Ok(intent);            
        }
    }
}
