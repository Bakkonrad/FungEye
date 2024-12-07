using FungEyeApi.Enums;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly static BlobContainerEnum blobContainer = BlobContainerEnum.Posts;

        public PostController(IPostsService postsService, IUserService userService, IBlobStorageService blobStorageService)
        {
            _postsService = postsService;
            _userService = userService;
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("addPost")]
        public async Task<IActionResult> AddPost([FromForm] int userId, [FromForm] string postJson, [FromForm] IFormFile? image = null)
        {
            try
            {
                if (!ValidateUserId(userId)) // only logged in users can add posts
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
                if (!ValidateUserId(userId)) // only post author can edit post
                {
                    return Forbid();
                }

                var post = JsonConvert.DeserializeObject<Post>(postJson) ?? throw new Exception("Cannot deserialize Post object");

                if (image != null) // if new image is uploaded delete old one and upload new one
                {
                    if (image.Length > 0)
                    {
                        if (!String.IsNullOrWhiteSpace(post.ImageUrl)) // if post has image delete it
                        {
                            await _blobStorageService.DeleteFile(post.ImageUrl, blobContainer);
                        }
                        var newImageUrl = await _blobStorageService.UploadFile(image, blobContainer);
                        post.ImageUrl = newImageUrl;
                    }
                }
                else // if no new image is uploaded check if user wants to delete old one
                {
                    if (post.DeletePhoto != null && post.DeletePhoto is true && post.ImageUrl != null)
                    {
                        await _blobStorageService.DeleteFile(post.ImageUrl, blobContainer);
                        post.ImageUrl = null;
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
        [HttpPost("deletePost")]
        public async Task<IActionResult> DeletePost([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false) // only admin ant post authors can delete other users posts
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
        [HttpPost("getPost")]
        public async Task<IActionResult> GetPost([FromForm] int userId, [FromForm] int postId)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var result = await _postsService.GetPost(postId, userId);
                return Ok(result);
            }
            catch(KeyNotFoundException)
            {
                return StatusCode(520, "Post not found");
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

                var result = await _postsService.AddPostReaction(postId, userId);
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

                var comment = JsonConvert.DeserializeObject<Comment>(commentJson) ?? throw new Exception("Cannot deserialize comment object");

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

                var comment = JsonConvert.DeserializeObject<Comment>(commentJson) ?? throw new Exception("Cannot deserialize comment object");

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
        [HttpPost("deleteComment")]
        public async Task<IActionResult> DeleteComment([FromForm] int userId, [FromForm] int commentId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) && admin == false) // only admin and comment authors can delete comments
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

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("report")]
        public async Task<IActionResult> Report([FromForm] int reporterId, [FromForm] int? postId = null, [FromForm] int? commentId = null)
        {
            try
            {
                if (!ValidateUserId(reporterId))
                {
                    return Forbid();
                }

                var result = await _postsService.Report(reporterId, postId, commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getReports")]
        public async Task<IActionResult> GetReports()
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!admin)
                {
                    return Forbid();
                }

                var result = await _postsService.GetReports();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [Consumes("multipart/form-data")]
        [HttpPost("markReportAsCompleted")]
        public async Task<IActionResult> MarkReportAsCompleted([FromForm] int userId, [FromForm] int reportId)
        {
            try
            {
                var userIdFromToken = int.Parse(GetUserIdFromToken());
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!ValidateUserId(userId) || !admin) // only admin can mark reports as completed
                {
                    return Forbid();
                }

                var result = await _postsService.MarkReportAsCompleted(reportId);
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
