﻿using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterUser(RegisterDto registerDto);
        Task<Result> LoginUser(LoginDto loginDto);
        Task<bool> IsUserAuthenticatedAsync(LoginDto loginDto);
        Task<Users> GetUserById(int id);
    }
}
