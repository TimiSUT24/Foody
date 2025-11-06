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

        [HttpPost("RawMaterial/create")]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 401)]
        public async Task<IActionResult> CreateRawMaterial(CreateRawMaterialDto request, CancellationToken ct)
        {
            var result = await _rawMaterialService.AddAsync(request, ct);
            return CreatedAtAction(nameof(CreateRawMaterial), new {id = request.FoodId}, result);
        }

        [HttpGet("RawMaterial/GetAll")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetRawMaterials([FromQuery] int foodId, CancellationToken ct)
        {
            var rawMaterials = await _rawMaterialService.GetAllAsync(foodId, ct);
            return Ok(rawMaterials);
        }

        [HttpGet("RawMaterial/Id")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> GetRawMaterialById([FromRoute] int foodId, CancellationToken ct)
        {
            var rawMaterial = await _rawMaterialService.GetByIdAsync(foodId, ct);
            return Ok(rawMaterial);
        }

    }
}
