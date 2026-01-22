using Application.Order.Dto.Request;
using Application.Order.Interfaces;
using Domain.Enum;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using System.Text.Json;

namespace Api.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;

        public OrderController (IOrderService orderService,IConfiguration config)
        {
            _orderService = orderService;              
            _config = config;
        }

        private Guid UserId =>
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id
                : throw new UnauthorizedAccessException("Ingen inloggad användare.");


        [HttpPost("create")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:400)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken ct)
        {
            var order = await _orderService.CreateAsync(UserId, request, ct);
            return Ok(order);
            
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _orderService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpGet("MyOrders")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> GetMyOrders(OrderStatus? status,CancellationToken ct)
        {
            var result = await _orderService.GetUserOrders(UserId, status, ct);
            return Ok(result);
        }

        [HttpGet("MyOrder{orderId:guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> GetMyOrder(Guid orderId, CancellationToken ct)
        {
            var result = await _orderService.GetUserOrder(UserId, orderId, ct);
            return Ok(result);
        }

        [HttpPatch("update-order")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> UpdateOrder(UpdateOrder request, CancellationToken ct)
        {
            var result = await _orderService.UpdateOrder(request, ct);
            return Ok(result);
        }
        

        [HttpPut("Cancel{orderId:guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> CancelMyOrder(Guid orderId, CancellationToken ct)
        {
            var result = await _orderService.CancelMyOrder(UserId, orderId, ct);
            return Ok(result);
        }

        [HttpPost("CalculateTax")]
        [ProducesResponseType(statusCode: 200)]
        public async Task<IActionResult> CalculateTax([FromBody] CartItemsDto cartItems, CancellationToken ct)
        {
            var result = await _orderService.CalculateTax(cartItems, ct);
            return Ok(result);
        }

        [HttpPost("retrieve-payment-intent")]
        public async Task<IActionResult> RetrievePaymentIntent([FromBody] RetrievePaymentIntentRequest request)
        {
            if (string.IsNullOrEmpty(request.ClientSecret))
                return BadRequest("ClientSecret is required.");

            var service = new PaymentIntentService();

            // Extract the PaymentIntent ID from the client secret
            var paymentIntentId = request.ClientSecret.Split("_secret")[0];

            var paymentIntent = await service.GetAsync(paymentIntentId);

            return Ok(paymentIntent); // full PaymentIntent including metadata and shipping
        }

    }
}
