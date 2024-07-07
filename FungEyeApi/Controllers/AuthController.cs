using Azure.Core;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobStorageService;


        public AuthController(IAuthService authService, IBlobStorageService blobStorageService)
        {
            _authService = authService;
            _blobStorageService = blobStorageService;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromForm] string userJson, [FromForm] IFormFile? image = null)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(userJson) ?? throw new Exception("Error during deserializing userJson");
                
                if (image == null || image.Length == 0)
                {
                    user.ImageUrl = "https://zestyappblob.blob.core.windows.net/zestyappimages/placeholder.png";
                }
                else
                {
                    var imageUrl = await _blobStorageService.UploadFile(image) ?? throw new Exception("Error during uploding the photo");
                    user.ImageUrl = imageUrl;
                }
                bool result = await _authService.Register(user);
                if (result)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return BadRequest("User registration failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUser loginUser)
        {
            try
            {
                var token = await _authService.Login(loginUser);
                return token != null ? Ok(token) : BadRequest("User login failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
