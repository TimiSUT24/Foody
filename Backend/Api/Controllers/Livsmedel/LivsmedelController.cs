using Application.Livsmedel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Livsmedel
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivsmedelController : ControllerBase
    {
        private readonly ILivsmedelService _livsmedelService;
        private readonly ILivsmedelImportService _livsmedelImportService;
        public LivsmedelController(ILivsmedelService livsmedelService, ILivsmedelImportService livsmedelImportService)
        {
            _livsmedelService = livsmedelService;
            _livsmedelImportService = livsmedelImportService;
        }

        [HttpPost("/livsmedel")]
        public async Task<IActionResult> ImportLivsmedel(int offset, int limit, int sprak)
        {
            var product = await _livsmedelImportService.ImportLivsmedelBatchAsync(offset,limit,sprak);
            return Ok(product);
        }
    }
}
