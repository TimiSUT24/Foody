using Application.Classification.Dto.Request;
using Application.Classification.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Classification
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        private readonly IClassificationService _classificationService;

        public ClassificationController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }

        [HttpPost("create")]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> CreateClassification(CreateClassificationDto request, CancellationToken ct)
        {
            var result = await _classificationService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateClassification), new { id = request.FoodId }, result);
        }

        [HttpGet("GetAllByProduct/{foodId}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> GetClassificationByProduct(int foodId, CancellationToken ct)
        {
            var result = await _classificationService.GetAllAsync(foodId, ct);
            return Ok(result);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> GetClassificationById([FromRoute] int Id , CancellationToken ct)
        {
            var result = await _classificationService.GetByIdAsync(Id, ct);
            return Ok(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> UpdateClassification([FromBody] UpdateClassificationDto request, CancellationToken ct)
        {
            var result = await _classificationService.Update(request, ct);
            return Ok(result);
        }

        [HttpDelete("delete/{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> DeleteClassification([FromRoute] int Id, CancellationToken ct)
        {
            var result = await _classificationService.Delete(Id, ct);
            return Ok(result); 
        }

    }
}
