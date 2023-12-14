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
            var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_jwtSettings.KeyAccess);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                       new Claim(ClaimTypes.Name, username)
                  }),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                 var accessToken = tokenHandler.WriteToken(token);

                _httpContextAccessor.HttpContext.Items["Authorization"] = accessToken;

                return accessToken;
        }

        public string GenerateRefreshToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyRefresh = Encoding.ASCII.GetBytes(_jwtSettings.KeyRefresh);

            var tokenDescriptorRefresh = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                          new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyRefresh), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenRefresh = tokenHandler.CreateToken(tokenDescriptorRefresh);

            var refreshToken = tokenHandler.WriteToken(tokenRefresh);
        
           _httpContextAccessor.HttpContext.Items["RefreshToken"] = refreshToken;

            return refreshToken;
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

                nameUser.Token = RefreshToken.ToString();

                var findUser = await _userRepository.UpdateAsync(nameUser);
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
