﻿using FungEyeApi.Interfaces;
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
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}