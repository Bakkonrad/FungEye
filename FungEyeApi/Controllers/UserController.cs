using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBlobStorageService _blobStorageService;

        public UserController(IUserService userService, IBlobStorageService blobStorageService)
        {
            _userService = userService;
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [HttpPost("removeAccount")]
        public async Task<IActionResult> RemoveAccount([FromBody] int userId, string token)
        {
            try
            {
                var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdFromToken == null || userIdFromToken != userId.ToString())
                {
                    return Forbid();
                }

                bool result = await _userService.RemoveAccount(userId);
                if (result)
                {
                    return Ok("Account deleted successfully.");
                }
                else
                {
                    return BadRequest("Account deletion failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("uploadPhoto")]
        public async Task<IActionResult> UploadPhoto([FromForm] int userId, [FromForm] IFormFile? image = null)
        {
            try
            {
                //var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                //bool result = await _blobStorageService.UploadFile(userId, image);
                bool result = true;
                if (result)
                {
                    return Ok("Account deleted successfully.");
                }
                else
                {
                    return BadRequest("Account deletion failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("getProfile")]
        public async Task<IActionResult> GetProfile([FromBody] int userId)
        {
            if (!ValidateUserId(userId))
            {
                return Forbid();
            }

            var user = await _userService.GetUserProfile(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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

    }
}
