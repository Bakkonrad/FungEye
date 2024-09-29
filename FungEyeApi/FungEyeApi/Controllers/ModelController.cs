using FungEyeApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            this._modelService = modelService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromForm] IFormFile image)
        {
            try
            {
                var result = await _modelService.Predict(image);
                if (result != null)
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }
                else
                {
                    return BadRequest("Prediction failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
