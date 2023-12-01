using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<Users> _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IRepository<Users> userRepository, IMapper mapper, IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;

            _mapper = mapper;

            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> RegisterUser(RegisterDto registerDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var userMap = _mapper.Map<Users>(registerDto);

            var refreshToken = GenerateJwtToken(userMap);
           
            var accessToken = CreateRefreshToken();

            userMap.Password = hashedPassword;

            userMap.Token = refreshToken;

            var resultRegister = await _userRepository.AddAsync(userMap);

            _httpContextAccessor.HttpContext.Items["CookieValue"] = refreshToken;

            if (resultRegister) 
               return new Result { Success = true, Message = "User registration successful.", Token = accessToken };

             return new Result { Success = true, Message = "User Not registered" };
        }

        public string GenerateJwtToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result> LoginUser(LoginDto loginDto)
        {
            var userMap = _mapper.Map<Users>(loginDto);

            var userPass =_userRepository.ChekeAndResultAsync(userMap).Result;

            var verifyPass = BCrypt.Net.BCrypt.EnhancedVerify(loginDto.password, userPass);

            var accessToken = GenerateJwtToken(userMap);
        
            if (!verifyPass)
                return new Result { Success = false, Message = "The password is wrong" };
           
            return new Result { Success = true, Message = "", Token =  accessToken };
        }

         public string CreateRefreshToken()
    {
        // A refresh token usually is just a random string.
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    }
}
