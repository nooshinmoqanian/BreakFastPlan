using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositories<Users> _userRepository;        
        public TokenService(IOptionsSnapshot<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor, IRepositories<Users> userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        public string GenerateAccessToken(string username)
        {
            var accessToken = GenerateToken(username, DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes), _jwtSettings.KeyAccess);
            _httpContextAccessor.HttpContext.Items["Authorization"] = accessToken;

             return accessToken;
        }

        public string GenerateRefreshToken(string username)
        {
            var refreshToken = GenerateToken(username, DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationDays), _jwtSettings.KeyRefresh);
           _httpContextAccessor.HttpContext.Items["RefreshToken"] = refreshToken;

            return refreshToken;
        }

        public string GenerateToken(string username, DateTime expiration, string key)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var keyToken = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                          new Claim(ClaimTypes.Name, username)
                    }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyToken), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandlerResult = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenHandlerResult);
        }

        public async Task<Users> GetToken(string token)
        {
            return await _userRepository.GetByTokenAsync(token);
        }

        public async Task<Result> UpdateToken(string username)
        {
            if (string.IsNullOrEmpty(username)) return new Result { Success = false, Message = "username cant be empty" };

            if (_httpContextAccessor.HttpContext.Items.TryGetValue("RefreshToken", out var RefreshToken))
            {
                var nameUser = await _userRepository.GetByNameAsync(username);

                if(nameUser == null) return new Result { Success = false, Message = "User not found." };

                nameUser.Token = RefreshToken.ToString();

                var updateUser = await _userRepository.UpdateAsync(nameUser);

                if(updateUser == null) return new Result { Success = false, Message = "Failed to update the user with new token." };
            }

            return new Result { Success = true, Message = "Update Token Successful" };
        }

        public async Task<bool> VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyRefresh = Encoding.ASCII.GetBytes(_jwtSettings.KeyRefresh);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyRefresh),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 
