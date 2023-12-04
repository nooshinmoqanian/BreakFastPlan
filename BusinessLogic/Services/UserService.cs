using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<Users> _userRepository;
        private readonly IMapper _mapper;
        public UserService(IRepositories<Users> repositories, IMapper mapper) 
        {
            _userRepository = repositories;
            _mapper = mapper;
        }

        public async Task<bool> CheckUserAuthenticationAsync(LoginDto loginDto)
        {
            if (loginDto.username != null)
            {
                var findUser = await _userRepository.GetByNameAsync(loginDto.username);

                if (findUser != null)
                {
                    if (loginDto.password != null)
                    {
                        var verifyPass = BCrypt.Net.BCrypt.Verify(loginDto.password, findUser.Password);

                        if (verifyPass) return true;
                    }
                }
            }
            return false;
        }

        public async Task<Result> LoginUser(LoginDto loginDto)
        {
            if (loginDto != null)
            {
                var userMap = _mapper.Map<Users>(loginDto);

                var chekeVerify = await CheckUserAuthenticationAsync(loginDto);

                if(chekeVerify)
                    return new Result { Success = true, Message = "you are login" };
            }

            return new Result { Success = false, Message = "The password is incorrect" };
        }

        public async Task<Result> RegisterUser(RegisterDto registerDto)
        {
            var userMap = _mapper.Map<Users>(registerDto);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            userMap.Password = hashedPassword;

            var resultRegister = await _userRepository.CreateAsync(userMap);

            if (resultRegister.Success == false)
                return new Result { Success = resultRegister.Success, Message = "User Registration Not Successful." };

            return new Result { Success = resultRegister.Success, Message = resultRegister.Message };
        }
    }
}
