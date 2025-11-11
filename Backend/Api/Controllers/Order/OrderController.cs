using Application.Order.Dto.Request;
using Application.Order.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController (IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid UserId =>
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id
                : throw new InvalidOperationException("Ingen inloggad användare.");

        [HttpPost("create")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:400)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken ct)
        {
            var result = await _orderService.CreateAsync(UserId, request, ct);
            return CreatedAtAction(nameof(CreateOrder), result);
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
        public async Task<IActionResult> GetMyOrders(CancellationToken ct)
        {
            var result = await _orderService.GetUserOrders(UserId, ct);
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

        [HttpPut("Cancel{orderId:guid}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> CancelMyOrder(Guid orderId, CancellationToken ct)
        {
            var result = await _orderService.CancelMyOrder(UserId, orderId, ct);
            return Ok(result);
        }

    }
}
