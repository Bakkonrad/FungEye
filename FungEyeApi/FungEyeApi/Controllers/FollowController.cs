using FungEyeApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly IAuthService _authService;
        private readonly IBlobStorageService _blobStorageService;

        public FollowController(IFollowService followService, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _followService = followService;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }

        [Authorize]
        [HttpPost("addFollow/{userId}/{followId}")]
        public async Task<IActionResult> AddFollow(int userId, int followId)
        {
            if (!ValidateUserId(userId))
            {
                return Forbid();
            }

            try
            {
                var result = await _followService.AddFollow(userId, followId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getFollows/{userId}")]
        public async Task<IActionResult> GetFollows(int userId)
        {

            try
            {
                var result = await _followService.GetFollows(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getFollowers/{userId}")]
        public async Task<IActionResult> GetFollowers(int userId)
        {

            try
            {
                var result = await _followService.GetFollowers(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("removeFollow/{userId}/{followId}")]
        public async Task<IActionResult> RemoveFollow(int userId, int followId)
        {
            if (!ValidateUserId(userId))
            {
                return Forbid();
            }

            try
            {
                var result = await _followService.RemoveFollow(userId, followId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("isFollowing/{userId}/{followId}")]
        public async Task<IActionResult> IsFollowing(int userId, int followId)
        {
            try
            {
                var result = await _followService.IsFollowing(userId, followId);
                return Ok(result);
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
