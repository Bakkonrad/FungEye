using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FungiAtlasController : ControllerBase
    {
        private readonly IFungiAtlasService _fungiAtlasService;
        private readonly IUserService _userService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly static BlobContainerEnum blobContainer = BlobContainerEnum.Fungies;



        public FungiAtlasController(IAuthService authService, IFungiAtlasService fungiAtlasService, IUserService userService, IBlobStorageService blobStorageService)
        {
            _fungiAtlasService = fungiAtlasService;
            _userService = userService;
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addFungi")]
        public async Task<IActionResult> AddFungi([FromForm] int userId, [FromForm] string fungiJson, [FromForm] IFormFileCollection? images)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false)
                {
                    return Forbid("Only admin can perform this action");
                }

                var fungi = JsonConvert.DeserializeObject<Fungi>(fungiJson) ?? throw new Exception("Cannot deserialize Fungi object");

                if (images != null && images.Count > 0)
                {
                    fungi.ImagesUrl = new List<string>();

                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                            fungi.ImagesUrl.Add(newImageUrl);
                        }
                    }
                }

                var result = await _fungiAtlasService.AddFungi(fungi);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("editFungi")]
        public async Task<IActionResult> EditFungi([FromForm] int userId, [FromForm] string fungiJson, [FromForm] IFormFileCollection? images)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false)
                {
                    return Forbid("Only admin can perform this action");
                }

                var fungi = JsonConvert.DeserializeObject<Fungi>(fungiJson) ?? throw new Exception("Cannot deserialize Fungi object");

                if (images != null && images.Count > 0)
                {
                    fungi.ImagesUrl ??= new List<string>();

                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                            fungi.ImagesUrl.Add(newImageUrl);
                        }
                    }
                }

                var result = await _fungiAtlasService.EditFungi(fungi);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("deleteFungi")]
        public async Task<IActionResult> DeleteFungi([FromForm] int userId, [FromForm] int fungiId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false)
                {
                    return Forbid("Only admin can perform this action");
                }

                var result = await _fungiAtlasService.DeleteFungi(fungiId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Consumes("multipart/form-data")]
        [HttpPost("getFungies")]
        public async Task<IActionResult> GetFungies([FromForm] int? page = null, [FromForm] string? search = null, [FromForm] int? userId = null)
        {
            try
            {
                var result = await _fungiAtlasService.GetFungies(userId, page, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("getFungi/{fungiId}")]
        public async Task<IActionResult> GetFungi(int fungiId, [FromBody] int? userId = null)
        {
            try
            {
                var result = await _fungiAtlasService.GetFungi(fungiId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("getFungiByName/{fungiName}")]
        public async Task<IActionResult> GetFungiByName(string fungiName, [FromBody] int? userId = null)
        {
            try
            {
                var result = await _fungiAtlasService.GetFungiByName(fungiName, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("addFungiToCollection")]
        public async Task<IActionResult> AddFungiToCollection([FromForm] int userId, [FromForm] int fungiId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _fungiAtlasService.AddFungiToCollection(userId, fungiId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("deleteFungiFromCollection")]
        public async Task<IActionResult> DeleteFungiFromCollection([FromForm] int userId, [FromForm] int fungiId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _fungiAtlasService.DeleteFungiFromCollection(userId, fungiId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private bool ValidateUserId(int userId)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken == null || userIdFromToken != userId.ToString())
            {
                return false;
            }

            return true;
        }

        private string GetUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
