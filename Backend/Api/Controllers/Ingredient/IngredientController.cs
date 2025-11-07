using Application.Ingredient.Dto.Request;
using Application.Ingredient.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Ingredient
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpPost("create")]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> CreateIngredient([FromBody] CreateIngredientDto request, CancellationToken ct)
        {
            var result = await _ingredientService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateIngredient), new { id = request.FoodId }, result);
        }

        [HttpGet("GetAllByProduct/{foodId:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetIngredientsByProduct([FromRoute] int foodId, CancellationToken ct)
        {
            var ingredients = await _ingredientService.GetAllAsync(foodId, ct);
            return Ok(ingredients);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetIngredientById([FromRoute] int Id, CancellationToken ct)
        {
            var ingredient = await _ingredientService.GetByIdAsync(Id, ct);
            return Ok(ingredient);
        }

        [HttpPut("update")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> UpdateIngredient([FromBody] UpdateIngredientDto request, CancellationToken ct)
        {
            var result = await _ingredientService.Update(request, ct);
            return Ok(result);
        }

        [HttpDelete("delete/{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int Id, CancellationToken ct)
        {
            var result = await _ingredientService.DeleteAsync(Id, ct);
            return Ok(result);
        }

    }
}
