﻿using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApi.ActionFilter;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("register")]
        [SetCookie]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registrationResult = await _userService.RegisterUser(registerDto);

                        if (registrationResult != null)
                return Ok(registrationResult);

            return BadRequest(ModelState);
        }
       
    }
}