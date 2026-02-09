using Application.Product.Dto.Request;
using Application.Product.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:409)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto request, CancellationToken ct)
        {
            var result = await _productService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateProduct), result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetSome")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        public async Task<IActionResult> GetProducts([FromQuery] int offset, [FromQuery] int limit, CancellationToken ct)
        {
            var products = await _productService.GetAsync(offset, limit, ct);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        public async Task<IActionResult> GetProductById([FromRoute] int id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);
            return Ok(product);
        }

        [HttpGet("details/{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetProductDetails([FromRoute] int id, CancellationToken ct)
        {
            var result = await _productService.GetProductDetailsById(id, ct);
            return Ok(result);
        }

        [HttpGet("filter")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> FilterProducts(string? name,string? brand, int? categoryId, int? subCategoryId, int? subSubCategoryId, decimal? price,bool? offer, CancellationToken ct, int page = 1, int pageSize = 25)
        {
            var result = await _productService.FilterProducts(name,brand,categoryId,subCategoryId,subSubCategoryId,price,offer,page,pageSize, ct);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto request, CancellationToken ct)
        {
            var result = await _productService.Update(request, ct);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(statusCode:200)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:401)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);
            return Ok(result);
        }

        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands([FromQuery] int? categoryId)
        {
            var result = await _productService.GetBrands(categoryId);
            return Ok(result);
        }
    }
}
