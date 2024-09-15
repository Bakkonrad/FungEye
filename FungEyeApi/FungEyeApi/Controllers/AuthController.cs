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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;


        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                if (await _authService.IsUsernameOrEmailUsed(user.Username, user.Email))
                {
                    return BadRequest("Username or email already in use.");
                }

                bool result = await _authService.RegisterUser(user);
                if (result)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    return BadRequest("User registration failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] User user)
        {
            try
            {
                if (await _authService.IsUsernameOrEmailUsed(user.Username, user.Email))
                {
                    return BadRequest("Username or email already in use.");
                }
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var admin = await _userService.IsAdmin(userIdFromToken);

                if (!admin)
                {
                    return BadRequest("Only admins can register other admins.");
                }

                bool result = await _authService.RegisterAdmin(user);
                if (result)
                {
                    return Ok("Admin registered successfully.");
                }
                else
                {
                    return BadRequest("Admin registration failed.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUser loginUser)
        {
            try
            {
                var token = await _authService.Login(loginUser);
                return token != null ? Ok(token) : BadRequest("User login failed");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (AccessViolationException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                if(ex.Message.Equals("Account is deleted"))
                {
                    return StatusCode(410, ex.Message);
                }
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("loginUser/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] string newPassword)
        {
            try
            {
                var result = await _authService.ChangePassword(userId, newPassword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
