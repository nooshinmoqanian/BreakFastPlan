using BusinessLogic.DTO;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        Result CreateToken(string username);

        Task <Result> UpdateToken(string username);

        Task<Users> GetToken(string token);

        Task<Users> VerifyToken(string token);
    }
}
