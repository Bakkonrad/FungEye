using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FungiAtlasController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;


        public FungiAtlasController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }


        //[Authorize]
        //[HttpPost("registerAdmin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] User user)
        //{
        //    try
        //    {
        //        if (await _authService.IsUsernameOrEmailUsed(user.Username, user.Email))
        //        {
        //            return BadRequest("Username or email already in use.");
        //        }
        //        var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //        var admin = await _userService.IsAdmin(userIdFromToken);

        //        if (!admin)
        //        {
        //            return BadRequest("Only admins can register other admins.");
        //        }

        //        bool result = await _authService.RegisterAdmin(user);
        //        if (result)
        //        {
        //            return Ok("Admin registered successfully.");
        //        }
        //        else
        //        {
        //            return BadRequest("Admin registration failed.");
        //        }
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error: " + ex.Message);
        //    }

        //}
    }
}
