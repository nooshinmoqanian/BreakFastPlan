using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
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
