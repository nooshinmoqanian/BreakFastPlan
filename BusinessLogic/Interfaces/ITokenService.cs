using BusinessLogic.DTO;
using DataAccess.Models;
using System.Security.Claims;

namespace BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        Result CreateToken(Users users);
        
        Task<bool> UpdateToken();
    }
}
