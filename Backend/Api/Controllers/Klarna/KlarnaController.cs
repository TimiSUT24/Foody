using Application.Order.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Api.Controllers.Klarna
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlarnaController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly HttpClient _httpClient;

        public KlarnaController(IOrderService orderService,HttpClient httpClient)
        {
            _orderService = orderService;
            _httpClient = httpClient;
        }

        [HttpPost("push/{orderId}")]
        public async Task<IActionResult> Push(Guid orderId, [FromBody] JsonElement payload, CancellationToken ct)
        {
            try
            {
                // Extract Klarna order status
                string status = payload.GetProperty("status").GetString();

                // Update order in your database
                await _orderService.UpdateStatusAsync(orderId, status,ct);

                return Ok(); // Klarna expects 200
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klarna push error: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet("klarna-confirm/{orderId}")]
        public async Task<IActionResult> KlarnaConfirm(Guid orderId,CancellationToken ct)
        {
            var order = await _orderService.GetByIdAsync(orderId,ct);
            if (order == null)
                return NotFound();
          
            return Ok(new
            {
                message = "Payment complete or in progress",
                orderId = order.Id,
                status = order.OrderStatus
            });
        }

        [HttpGet("klarna-terms")]
        public IActionResult KlarnaTerms()
        {
            return Content("Your terms and conditions go here", "text/html");
        }

    }
}
