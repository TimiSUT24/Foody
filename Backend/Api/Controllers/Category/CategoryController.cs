using Application.Category.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("tree")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetCategoryTree(CancellationToken ct)
        {
            var result = await _categoryService.GetCategoryTree(ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _categoryService.GetCategoryById(id, ct);
            return Ok(result);
        }

        [HttpGet("subCategory{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetSubCategoryById([FromRoute] int id)
        {
            var result = await _categoryService.GetSubCategoryById(id);
            return Ok(result);
        }


        [HttpGet("subSubCategory{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetSubSubCategoryById([FromRoute] int id)
        {
            var result = await _categoryService.GetSubSubCategoryById(id);
            return Ok(result);
        }
    }
}
