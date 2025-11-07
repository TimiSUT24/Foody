using Application.RawMaterial.Dto.Request;
using Application.RawMaterial.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.RawMaterial
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        private readonly IRawMaterialService _rawMaterialService;

        public RawMaterialController(IRawMaterialService rawMaterialService)
        {
            _rawMaterialService = rawMaterialService;
        }

        [HttpPost("create")]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> CreateRawMaterial(CreateRawMaterialDto request, CancellationToken ct)
        {
            var result = await _rawMaterialService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateRawMaterial), new {id = request.FoodId}, result);
        }

        [HttpGet("GetAllByProduct/{foodId:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetRawMaterialsByProduct([FromRoute] int foodId, CancellationToken ct)
        {
            var rawMaterials = await _rawMaterialService.GetAllAsync(foodId, ct);
            return Ok(rawMaterials);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetRawMaterialById([FromRoute] int Id, CancellationToken ct)
        {
            var rawMaterial = await _rawMaterialService.GetByIdAsync(Id, ct);
            return Ok(rawMaterial);
        }

        [HttpPut("update")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> UpdateRawMaterial([FromBody] UpdateRawMaterialDto request, CancellationToken ct)
        {
            var result = await _rawMaterialService.Update(request, ct);
            return Ok(result);
        }

        [HttpDelete("delete/{Id:int}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> DeleteRawMaterial([FromRoute]int Id, CancellationToken ct)
        {
            var result = await _rawMaterialService.Delete(Id, ct);
            return Ok(result);
        }
    }
}
