

using BusinessLogic.DTO;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterUser(RegisterDto registerDto);
    }
}
