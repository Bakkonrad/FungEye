using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Formats;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addPost")]
        public async Task<IActionResult> AddPost([FromForm]int userId, [FromForm] Post postJson, [FromForm] IFormFile? image)
        {
            try
            {
                var post = JsonConvert.DeserializeObject<Post>(postJson);

                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        var newImageUrl = await _blobStorageService.UploadFile(image);
                        userJson.ImageUrl = newImageUrl;

                    }
                }

                if (userJson.ImageUrl.Equals("changeToPlaceholder"))
                {
                    await _blobStorageService.DeleteFile(userJson.ImageUrl);
                    userJson.ImageUrl = "placeholder";
                }



                var updateUser = _userService.UpdateUser(userJson);
                return Ok();
            }
            catch (ArgumentException)
            {
                return StatusCode(405, "Username or email already in use");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
