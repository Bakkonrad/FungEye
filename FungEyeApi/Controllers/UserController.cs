using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FungEyeApi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("removeAccount")]
        public async Task<IActionResult> RemoveAccount([FromBody] int userId, string token)
        {
            try
            {
                bool result = await _userService.RemoveAccount(userId, token);
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
    }
}
