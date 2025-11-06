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

        [HttpGet("GetAll")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetIngredients([FromQuery] int foodId, CancellationToken ct)
        {
            var ingredients = await _ingredientService.GetAllAsync(foodId, ct);
            return Ok(ingredients);
        }

        [HttpGet("Id")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetIngredientById([FromRoute] int foodId, CancellationToken ct)
        {
            var ingredient = await _ingredientService.GetByIdAsync(foodId, ct);
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

        [HttpDelete("delete")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> DeleteIngredient([FromQuery] int foodId, CancellationToken ct)
        {
            var result = await _ingredientService.DeleteAsync(foodId, ct);
            return Ok(result);
        }

    }
}
