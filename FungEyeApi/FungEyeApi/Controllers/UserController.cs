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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobStorageService;

        public UserController(IUserService userService, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _userService = userService;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }

        [Authorize]
        [HttpPost("removeAccount/{userId}")]
        public async Task<IActionResult> RemoveAccount(int userId)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false)
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
        [Consumes("multipart/form-data")]
        [HttpPost("UpdateUserImage/{userId}")]
        public async Task<IActionResult> UpdateUserImage(int userId, [FromForm] IFormFile? image = null)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false)
                {
                    return Forbid();
                }

                if (image == null || image.Length == 0)
                {
                    return BadRequest("No file selected.");
                }
                
                var oldUrl = await _userService.GetUserImage(userId);
                await _blobStorageService.DeleteFile(oldUrl);
                var newImageUrl = await _blobStorageService.UploadFile(image);

                bool result = await _userService.UpdateUserImage(userId, newImageUrl);

                if (result)
                {
                    return Ok("Photo added successfully.");
                }
                else
                {
                    return BadRequest("Photo added failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("getProfile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var admin = await _userService.IsAdmin(userIdFromToken);

            if (!ValidateUserId(userId) && admin == false)
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

        [HttpPut("updateUser")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] string user,
            [FromForm] IFormFile? image = null)
        {
            try
            {
                var userJson = JsonConvert.DeserializeObject<User>(user);

                if (await _authService.IsUsernameOrEmailUsed(userJson.Username, userJson.Email))
                {
                    return BadRequest("Username or email already in use.");
                }

                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userJson.Id) && admin == false)
                {
                    return Forbid();
                }

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        if (!IsPlaceholder(userJson.ImageUrl))
                        {
                            await _blobStorageService.DeleteFile(userJson.ImageUrl);
                            var newImageUrl = await _blobStorageService.UploadFile(image);
                            userJson.ImageUrl = newImageUrl;
                        }
                    }
                }

                var updateUser = _userService.UpdateUser(userJson);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("getUsers")]
        public async Task<IActionResult> GetUsers([FromForm] int userId, [FromForm] int? page = null,
            [FromForm] string? search = null)
        {
            if (!ValidateUserId(userId))
            {
                return Forbid();
            }

            var admin = await _userService.IsAdmin(userId);

            if (admin == false)
            {
                return Forbid();
            }

            var user = await _userService.GetUsers(page, search);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //[Authorize]
        //[Consumes("multipart/form-data")]
        //[HttpPost("getUsers")]
        //public async Task<IActionResult> AddFriendship([FromForm] int userId, [FromForm] int friendId)
        //{
        //    if (!ValidateUserId(userId))
        //    {
        //        return Forbid();
        //    }

        //    var user = await _userService.GetUsers();
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
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

        private static bool IsPlaceholder(string? imageurl = null)
        {
            return false;
        }


        [Authorize]
        [HttpPost("banUser/{userId}/{banOption}")]
        public async Task<IActionResult> BanUser(int userId, short banOption)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (admin == false)
                {
                    return Forbid();
                }

                bool result = await _userService.BanUser(userId, (BanOptionEnum)banOption);
                if (result)
                {
                    return Ok("User banned successfully.");
                }
                else
                {
                    return BadRequest("User banning failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}