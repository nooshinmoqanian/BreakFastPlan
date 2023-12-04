using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public UserService(IRepositories<Users> repositories, IMapper mapper, ITokenService tokenService) 
        {
            _userRepository = repositories;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<bool> CheckUserAuthenticationAsync(LoginDto loginDto)
        {
            if (!string.IsNullOrEmpty(loginDto.username))
            {
                var findUser = await _userRepository.GetByNameAsync(loginDto.username);

                if (findUser != null && !string.IsNullOrEmpty(loginDto.password))
                {
                    var verifyPass = BCrypt.Net.BCrypt.Verify(loginDto.password, findUser.Password);

                    return verifyPass;
                }
            }

            return false;
        }
        public async Task<Result> LoginUser(LoginDto loginDto)
        {
            if (loginDto != null)
            {
                var userMap = _mapper.Map<Users>(loginDto);

                var accessToken = _tokenService.CreateTokrn(userMap).AccessToken;

                var refreshToken = _tokenService.CreateTokrn(userMap).RefreshToken;

                var chekeVerify = await CheckUserAuthenticationAsync(loginDto);

                if(chekeVerify)
                    return new Result { Success = true, Message = "you are login", AccessToken = accessToken, RefreshToken = refreshToken};
            }

            return new Result { Success = false, Message = "The password is incorrect" };
        }

        public async Task<Result> RegisterUser(RegisterDto registerDto)
        {
            var userMap = _mapper.Map<Users>(registerDto);

            var accessToken = _tokenService.CreateTokrn(userMap).AccessToken;

            var refreshToken = _tokenService.CreateTokrn(userMap).RefreshToken;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            userMap.Password = hashedPassword;

            var resultRegister = await _userRepository.CreateAsync(userMap);

            if (resultRegister.Success == false)
                return new Result { Success = resultRegister.Success, Message = "User Registration Not Successful." };

            return new Result { Success = resultRegister.Success, Message = resultRegister.Message };
        }
    }
}
