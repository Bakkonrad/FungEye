﻿using Azure.Core;
using FungEyeApi.Interfaces;
using FungEyeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FungEyeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                bool result = await _authService.Register(user);
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