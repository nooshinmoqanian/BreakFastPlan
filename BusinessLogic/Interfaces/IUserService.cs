using BusinessLogic.DTO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
       Task<Result> RegisterUser(RegisterDto registerDto);
       Task<Result> LoginUser(LoginDto loginDto);

        string GenerateJwtToken(Users user);
        string CreateRefreshToken();
    }
}
