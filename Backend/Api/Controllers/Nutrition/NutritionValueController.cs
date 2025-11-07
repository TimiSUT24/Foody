using Application.NutritionValue.Dto.Request;
using Application.NutritionValue.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Nutrition
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionValueController : ControllerBase
    {
        private readonly INutritionValueService _nutritionValueService;

        public NutritionValueController(INutritionValueService nutritionValueService)
        {
            _nutritionValueService = nutritionValueService;
        }

        [HttpPost("create")]
        [ProducesResponseType(statusCode:201)]
        [ProducesResponseType(statusCode:404)]
        [ProducesResponseType(statusCode:400)]
        public async Task<IActionResult> CreateRawMaterial(CreateNutritionValueDto request, CancellationToken ct)
        {
            var result = await _nutritionValueService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateRawMaterial), new { Id = request.FoodId }, result);
        }

        [HttpGet("GetAllByProduct/{foodId}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetAllByProduct([FromRoute] int foodId, CancellationToken ct)
        {
            var result = await _nutritionValueService.GetAllAsync(foodId, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _nutritionValueService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> Update([FromBody] UpdateNutritionValueDto request, CancellationToken ct)
        {
            var result = await _nutritionValueService.Update(request, ct);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var result = await _nutritionValueService.Delete(id, ct);   
            return Ok(result);
        }

    }
}
