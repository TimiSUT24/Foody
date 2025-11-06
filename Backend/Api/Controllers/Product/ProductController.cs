using Application.Product.Dto.Request;
using Application.Product.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/create")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:409)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto request, CancellationToken ct)
        {
            var result = await _productService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateProduct), new { id = request.Id }, result);
        }

        [HttpGet("/Products")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        public async Task<IActionResult> GetProducts([FromQuery] int offset, [FromQuery] int limit, CancellationToken ct)
        {
            var products = await _productService.GetAsync(offset, limit, ct);
            return Ok(products);
        }

        [HttpGet("/Products/{id}")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        public async Task<IActionResult> GetProductById([FromRoute] int id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);
            return Ok(product);
        }

        [HttpDelete("/Products/{id}")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);
            return Ok(result);
        }

        [HttpPut("/Products/{id}")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDto request, CancellationToken ct)
        {
            var result = await _productService.Update(id, request, ct);
            return Ok(result);
        }

    }
}
