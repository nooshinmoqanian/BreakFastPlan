using BusinessLogic.DTO;
using DataAccess.Models;
using Shared.Dto;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterUser(RegisterDto registerDto);
        Task<Result> LoginUser(LoginDto loginDto);
        Task<AuthenticationResult> CheckUserAuthenticationAsync(LoginDto loginDto);
        Task<Users> GetUserById(int id);
    }
}
