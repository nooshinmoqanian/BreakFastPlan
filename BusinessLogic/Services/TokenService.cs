using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TokenService(JwtSettings jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public Result CreateToken(Users users)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKeyAccess = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.KeyAccess));

           var securityKeyRefresh = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.KeyRefresh));

           var credentials = new SigningCredentials(securityKeyAccess, SecurityAlgorithms.HmacSha256);

           var credentialsRefresh = new SigningCredentials(securityKeyRefresh, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, users.Username),
            new Claim(JwtRegisteredClaimNames.Email, users.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                    signingCredentials: credentials
                );

            var refreshToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentialsRefresh
                );

            string access = tokenHandler.WriteToken(accessToken);

            string refresh = tokenHandler.WriteToken(refreshToken);

            _httpContextAccessor.HttpContext.Items["RefreshToken"] = access;

            _httpContextAccessor.HttpContext.Items["Authentication"] = refresh;

            return new Result { Success = true, Message = "Tokens were created successfully", AccessToken = access, RefreshToken = refresh };
        }

        public Task<bool> UpdateToken()
        {
            throw new NotImplementedException();
        }
    }
}
