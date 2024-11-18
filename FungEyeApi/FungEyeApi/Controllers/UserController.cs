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
        private readonly static BlobContainerEnum blobContainer = BlobContainerEnum.Users;
        public UserController(IUserService userService, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _userService = userService;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }

        [Authorize]
        [HttpDelete("removeAccount/{userId}")]
        public async Task<IActionResult> RemoveAccount(int userId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false) // if user is not admin and userId is not the same as the one in token
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
        [HttpGet("retrieveAccount/{userId}")]
        public async Task<IActionResult> RetrieveAccount(int userId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (admin == false)
                {
                    return Forbid();
                }

                bool result = await _userService.RetrieveAccount(userId);
                if (result)
                {
                    return Ok("Account retrieved successfully.");
                }
                else
                {
                    return BadRequest("Account retrieve failed.");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("User not found"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPut("updateUserImage/{userId}")]
        public async Task<IActionResult> UpdateUserImage(int userId, [FromForm] IFormFile? image = null)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
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

                if (!IsPlaceholder(oldUrl)) // if old image is not placeholder then delete it
                {
                    bool deleteResult = await _blobStorageService.DeleteFile(oldUrl, blobContainer);
                }
                var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);

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
        [HttpGet("getProfile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var user = await _userService.GetUserProfile(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [Authorize]
        [HttpGet("getSmallProfile/{userId}")]
        public async Task<IActionResult> GetSmallProfile(int userId) //Endpoint for getting user profile with less data (for example for posts)
        {
            try
            {
                var user = await _userService.GetSmallUserProfile(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("updateUser")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] string userJson,
            [FromForm] IFormFile? image = null)
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(userJson) ?? throw new Exception("Error during deserializing User object");

                if (await _authService.IsUsernameOrEmailUsed(user.Username, user.Email, user.Id))
                {
                    return BadRequest("Username or email already in use.");
                }

                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(user.Id) && admin == false) // if user is not admin and userId is not the same as the one in token
                {
                    return Forbid();
                }

                if (image != null) // if image is uploaded
                {
                    if (image.Length > 0)
                    {
                        if (!String.IsNullOrWhiteSpace(user.ImageUrl) && !IsPlaceholder(user.ImageUrl)) // if user has image and it is not placeholder then delete it
                        {
                            await _blobStorageService.DeleteFile(user.ImageUrl, blobContainer);
                        }
                        var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                        user.ImageUrl = newImageUrl;

                    }
                }

                if (user.ImageUrl != null && user.ImageUrl.Equals("changeToPlaceholder")) // if user wants to delete image then delete it and set to placeholder
                {
                    await _blobStorageService.DeleteFile(user.ImageUrl, blobContainer);
                    user.ImageUrl = "placeholder";
                }

                var updateUser = await _userService.UpdateUser(user);
                return Ok();
            }
            catch (ArgumentException) // if username or email is already in use
            {
                return StatusCode(405, "Username or email already in use");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("getUsers")]
        public async Task<IActionResult> GetUsers([FromForm] int userId, [FromForm] int? page = null, [FromForm] string? search = null)
        {
            try
            {
                if (!ValidateUserId(userId))
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
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



        [Authorize]
        [HttpPost("banUser/{userId}/{banOption}")]
        public async Task<IActionResult> BanUser(int userId, short banOption)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (admin == false) // if user is not admin then forbid
                {
                    return Forbid();
                }

                var result = await _userService.BanUser(userId, (BanOptionEnum)banOption);
                if (result != null)
                {
                    return Ok(result);
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

        private bool ValidateUserId(int userId)
        {
            var userIdFromToken = GetUserIdFromToken();
            if (userIdFromToken == null || userIdFromToken != userId.ToString())
            {
                return false;
            }

            return true;
        }


        private static bool IsPlaceholder(string? imageurl)
        {
            return imageurl == "placeholder" ? true : false;
        }

        private string GetUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}