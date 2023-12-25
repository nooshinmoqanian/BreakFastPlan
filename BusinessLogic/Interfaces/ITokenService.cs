using BusinessLogic.DTO;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, DateTime expiration, string key);

        string GenerateAccessToken(string username);

        string GenerateRefreshToken(string username);

        Task <Result> UpdateToken(string username);

        Task<Users> GetToken(string token);

        Task<bool> VerifyToken(string token);
    }
}
