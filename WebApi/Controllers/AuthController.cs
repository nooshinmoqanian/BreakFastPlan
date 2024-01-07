using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ActionFilter;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        
        private readonly JwtSettings _jwtSettings;

        public AuthController(IUserService userService, ITokenService tokenService, JwtSettings jwtSettings)
        {
          _userService = userService;
          _tokenService = tokenService;
          
          _jwtSettings = jwtSettings;
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

            var tokenUpdate = await _tokenService.UpdateToken(loginDto.username);

          

            if (tokenUpdate.Success == false)
                return BadRequest(ModelState);

            if (LoginResult != null)
                return Ok(LoginResult);

            return BadRequest(ModelState);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (HttpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                string cookieValue = refreshToken.ToString();

                if (string.IsNullOrEmpty(cookieValue))
                {
                    return Unauthorized("You are not logged in");
                }

                var findToken = await _tokenService.GetToken(cookieValue);

                if(findToken == null) return Unauthorized("You are not logged in");

                var verify = await _tokenService.VerifyToken(cookieValue);

                if (!verify) return Unauthorized("You are not logged in");

                var createAccess =  _tokenService.GenerateAccessToken(findToken.Username, findToken.Role);

                return Ok(verify);
            }

            return Unauthorized();

        }

        [HttpGet("test")]
        [Authorize]
        public string test()
        {
            return " nooshin gole golab";
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public string admin()
        {
            return "admin wellcom";
        }
    }
}
