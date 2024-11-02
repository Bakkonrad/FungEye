using FungEyeApi.Enums;
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
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly static BlobContainerEnum blobContainer = BlobContainerEnum.Posts;


        public PostController(IPostsService postsService, IBlobStorageService blobStorageService)
        {
            _postsService = postsService;
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addPost")]
        public async Task<IActionResult> AddPost([FromForm] int userId, [FromForm] string postJson, [FromForm] IFormFile? image = null)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var post = JsonConvert.DeserializeObject<Post>(postJson) ?? throw new Exception("Cannot deserialize Post object");

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                        post.ImageUrl = newImageUrl;
                    }
                }

                post.UserId = userId;

                var result = _postsService.AddPost(post);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPut("editPost")]
        public async Task<IActionResult> EditPost([FromForm] int userId, [FromForm] string postJson, [FromForm] IFormFile? image)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var post = JsonConvert.DeserializeObject<Post>(postJson) ?? throw new Exception("Cannot deserialize Post object");

                if (image != null)
                {
                    if (image.Length > 0)
                    {
                        if (!String.IsNullOrWhiteSpace(post.ImageUrl))
                        {
                            await _blobStorageService.DeleteFile(post.ImageUrl, blobContainer);
                        }
                        var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                        post.ImageUrl = newImageUrl;
                    }
                }
                else
                {
                    if (post.DeletePhoto != null && post.ImageUrl != null)
                    {
                        if (post.DeletePhoto is true)
                        {
                            await _blobStorageService.DeleteFile(post.ImageUrl, blobContainer);
                            post.ImageUrl = null;
                        }
                    }
                }

                var result = await _postsService.EditPost(post);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpDelete("deletePost")]
        public async Task<IActionResult> DeletePost([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.DeletePost(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("getPosts")]
        public async Task<IActionResult> GetPosts([FromForm] int userId, [FromForm] int postsFilter, [FromForm] int? page = null)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.GetPosts((PostsFilter)postsFilter, userId, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addPostReaction")]
        public async Task<IActionResult> AddPostReaction([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.AddPostReaction(userId, postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("deletePostReaction")]
        public async Task<IActionResult> DeletePostReaction([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.DeletePostReaction(userId, postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("getComments")]
        public async Task<IActionResult> GetComments([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.GetComments(postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment([FromForm] int userId, [FromForm] string commentJson)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var comment = JsonConvert.DeserializeObject<Comment>(commentJson) ?? throw new Exception("Cannot deserialize Post object");

                comment.User = new FollowUser { Id = userId };

                var result = await _postsService.AddComment(comment);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPut("editComment")]
        public async Task<IActionResult> EditComment([FromForm] int userId, [FromForm] string commentJson)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var comment = JsonConvert.DeserializeObject<Comment>(commentJson) ?? throw new Exception("Cannot deserialize Post object");

                var result = await _postsService.EditComment(comment);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpDelete("deleteComment")]
        public async Task<IActionResult> DeleteComment([FromForm] int userId, [FromForm] int commentId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.DeleteComment(commentId);
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
    }
}
