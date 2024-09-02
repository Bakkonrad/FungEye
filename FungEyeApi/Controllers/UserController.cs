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
        private readonly IBlobStorageService _blobStorageService;

        public UserController(IUserService userService, IBlobStorageService blobStorageService)
        {
            _userService = userService;
            _blobStorageService = blobStorageService;
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

        //[Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("UpdateUserImage/{userId}")]
        public async Task<IActionResult> UpdateUserImage(int userId, [FromForm] IFormFile? image = null)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                if (image == null || image.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                //string oldImageUrl = await _userService.GetUserImage(userId); //DODAC USUWANIE STAREGO ZDJECIA

                string imageUrl = await _blobStorageService.UploadFile(image);
                bool result = await _userService.UpdateUserImage(userId, imageUrl);

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

        [HttpPut("updateUser/{userId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, [FromForm] string userJson,
            [FromForm] IFormFile? image = null)
        {
            try
            {
                //NIE KAZAĆ PRZEKAZYWAĆ USERA W OBIEKCIE JSON TYLKO POBIERAĆ IMAGE URL Z BAZY I NA TEJ PODSTAWIE ZMIENIAC ZDJECIE



                var user = JsonConvert.DeserializeObject<User>(userJson);

                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                if (image != null || image.Length > 0)
                {
                    await _blobStorageService.DeleteFile(user.ImageUrl);
                    var newImageUrl = await _blobStorageService.UploadFile(image);
                    user.ImageUrl = newImageUrl;
                }

                user.Id = userId;

                var updateUser = _userService.UpdateUser(user);
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
    }
}