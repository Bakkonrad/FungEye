using FungEyeApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FungEyeApi.Controllers
{
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        //[HttpPost("removeAccount")]
        //public async Task<IActionResult> RemoveAccount([FromBody] int userId, string token)
        //{
        //    try
        //    {
        //        bool result = await _postsService.RemoveAccount(userId, token);
        //        if (result)
        //        {
        //            return Ok("Account deleted successfully.");
        //        }
        //        else
        //        {
        //            return BadRequest("Account deletion failed.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error: " + ex.Message);
        //    }
        //}
    }
}
