using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using FungEyeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                    await _authService.SendSetAdminPasswordEmail(user.Email);
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
                return StatusCode(400, ex.Message);
            }
            catch (AccessViolationException ex)
            {
                return StatusCode(409, ex.Message);
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

        [HttpPost("sendResetPasswordEmail")]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmail resetPasswordModel)
        {
            try
            {
                if(!await _authService.IsUsernameOrEmailUsed(null, resetPasswordModel.Email))
                {
                    return StatusCode(501 ,"User not found");
                }

                if (string.IsNullOrWhiteSpace(resetPasswordModel.Email))
                {
                    return BadRequest("Email is required");
                }
                var result = await _authService.SendResetPasswordEmail(resetPasswordModel.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("resetPassword/{userId}")]
        public async Task<IActionResult> ResetPassword(int userId, [FromBody] ResetPassword resetPasswordModel)
        {
            try
            {
                if (!ValidateUserId(userId))
                {
                    return Forbid();
                }

                var user = await _userService.GetUserProfile(userId);
                if (user == null)
                {
                    return BadRequest("Invalid token");
                }

                if(string.IsNullOrWhiteSpace(resetPasswordModel.Password))
                {
                    return BadRequest("Password is required");
                }

                var result = await _authService.ChangePassword(userId, resetPasswordModel.Password);

                return Ok("Password reset successfully");
            }
            catch (Exception ex)
            {
                if(ex is ArgumentException)
                {
                    return StatusCode(400, "User not found" + ex.Message);
                }

                return BadRequest("Invalid token");
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
