using System.Collections;
using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FungEyeApi.Data.Entities;
using Newtonsoft.Json;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FungiAtlasController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFungiAtlasService _fungiAtlasService;
        private readonly IUserService _userService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly static BlobContainerEnum blobContainer = BlobContainerEnum.Fungies;



        public FungiAtlasController(IAuthService authService, IFungiAtlasService fungiAtlasService, IUserService userService, BlobStorageService blobStorageService)
        {
            _authService = authService;
            _fungiAtlasService = fungiAtlasService;
            _userService = userService;
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addFungi")]
        public async Task<IActionResult> AddFungi([FromForm] int userId, [FromForm] string fungiJson, [FromForm] IFormFile? image)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);
                
                if (!ValidateUserId(userId) || admin == false)
                {
                    return Forbid();
                }
                
                var fungi = JsonConvert.DeserializeObject<Fungi>(fungiJson) ?? throw new Exception("Cannot deserialize Fungi object");

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        var newImageUrl = await _blobStorageService.UploadFile(image, Enums.BlobContainerEnum.Fungies);
                        fungi.ImagesUrl = new List<string>() { newImageUrl };
                    }
                }

                var result = _fungiAtlasService.AddFungi(fungi);
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
        public async Task<IActionResult> EditFungi([FromForm] int userId, [FromForm] string fungiJson, [FromForm] IFormFile? image)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);
                
                if (!ValidateUserId(userId) || admin == false)
                {
                    return Forbid();
                }
                
                var fungi = JsonConvert.DeserializeObject<Fungi>(fungiJson) ?? throw new Exception("Cannot deserialize Fungi object");

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        if (!String.IsNullOrWhiteSpace(fungi.ImagesUrl[0]))
                        {
                            await _blobStorageService.DeleteFile(fungi.ImagesUrl[0], blobContainer);
                        }
                        var newImageUrl = await _blobStorageService.UploadFile(image, Enums.BlobContainerEnum.Fungies);
                        fungi.ImagesUrl[0] = newImageUrl;
                    }
                }

                var result = _fungiAtlasService.EditFungi(fungi);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpDelete("deleteFungi")]
        public async Task<IActionResult> DeleteFungi([FromForm] int userId, [FromForm] int fungiId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _fungiAtlasService.DeleteFungi(fungiId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpGet("getFungies")]
        public async Task<IActionResult> GetFungies([FromForm] int userId, [FromForm] int fungiesFilter, [FromForm] int? page = null)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _fungiAtlasService.GetFungies(page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        [Authorize]
        [HttpPost("saveFungi")]
        public async Task<IActionResult> SaveFungi([FromForm] int userId, [FromForm] int fungiId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _fungiAtlasService.SaveFungi(userId, fungiId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        
        //}
        private bool ValidateUserId(int userId)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken == null || userIdFromToken != userId.ToString())
            {
                return false;
            }

            return true;
        }
    }
}
