using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, DateTime expiration, string key, string role);

        string GenerateAccessToken(string username, string role);

        string GenerateRefreshToken(string username, string role);

        Task <Result> UpdateToken(string username);

        Task<Users> GetToken(string token);

        Task<bool> VerifyToken(string token);
    }
}
