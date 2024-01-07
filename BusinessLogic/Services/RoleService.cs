using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepositories<Users> _userRepository;
        private readonly JwtSettings _jwtSettings;

        public RoleService(IRepositories<Users> userRepository, IOptionsSnapshot<JwtSettings> jwtSettings) 
        {
            userRepository = _userRepository;
            _jwtSettings = jwtSettings.Value;

        }
        public string GetRoleFromToken(string token, string signKey)
        {
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(signKey);
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var roleClaim = jwtToken.Claims.First(claim => claim.Type == "role");

                    return roleClaim.Value;
                }
                catch(SecurityTokenExpiredException)
                {
                    return "TokenExpired";
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
