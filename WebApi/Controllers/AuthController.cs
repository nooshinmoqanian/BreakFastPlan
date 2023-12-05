using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [SetTokenCookie]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registrationResult = await _userService.RegisterUser(registerDto);

            if (registrationResult != null)
                return Ok(registrationResult);

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [SetTokenCookie]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var LoginResult = await _userService.LoginUser(loginDto);

            if (LoginResult != null)
                return Ok(LoginResult);

            return BadRequest(ModelState);
        }
    }
}
